using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Transactions;
using System.Web;
using VirtualTourCore.Common.Helper;
using VirtualTourCore.Common.Utilities;
using VirtualTourCore.Core.Interfaces;
using VirtualTourCore.Core.Models;

namespace VirtualTourCore.Core.Services
{
    public class AdminService : IAdminService
    {
        private IClientRepository _clientRepository;
        private IRegistrationCodeRepository _registrationCodeRepository;
        private ILocationRepository _locationRepository;
        private IAreaRepository _areaRepository;
        private ITourRepository _tourRepository;
        private IAssetStoreRepository _assetStoreRepository;
        private IFileService _fileService;
        private ISecurityUserClientRepository _securityUserClientRepository;

        public AdminService(
            IClientRepository clientRepository,
            IRegistrationCodeRepository registrationCodeRepository,
            ILocationRepository locationRepository,
            IAreaRepository areaRepository,
            ITourRepository tourRepository,
            IAssetStoreRepository assetStoreRepository,
            IFileService fileService,
            ISecurityUserClientRepository securityUserClientRepository
            )
        {
            _clientRepository = clientRepository;
            _registrationCodeRepository = registrationCodeRepository;
            _locationRepository = locationRepository;
            _areaRepository = areaRepository;
            _tourRepository = tourRepository;
            _assetStoreRepository = assetStoreRepository;
            _fileService = fileService;
            _securityUserClientRepository = securityUserClientRepository;
        }
        public int? CreateClient(Client client, HttpPostedFileBase logo, HttpPostedFileBase profile)
        {
            client.Guid = Guid.NewGuid();
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions()
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = CommonConfiguration.Configuration.TransactionScope_Timeout
            }))
            {
                var clientId = _clientRepository.Create(client);

                var logoUploadSuccess = string.IsNullOrWhiteSpace(logo.FileName);
                var profileUploadSuccess = string.IsNullOrWhiteSpace(profile.FileName);

                if (clientId != null)
                {
                    var baseLogoAssetId = client.AssetLogoId;
                    if (!logoUploadSuccess)
                    {
                        client.AssetLogoId = ProcessFileUpload(clientId.Value, client.CreateUserId.Value, client.AssetLogoId, logo);
                        if(baseLogoAssetId != client.AssetLogoId)
                        {
                            logoUploadSuccess = true;
                        }
                    }
                    var baseProfileAssetId = client.AssetProfileId;
                    if (!profileUploadSuccess)
                    {
                        client.AssetProfileId = ProcessFileUpload(clientId.Value, client.CreateUserId.Value, client.AssetProfileId, profile);
                        if (baseProfileAssetId != client.AssetProfileId)
                        {
                            profileUploadSuccess = true;
                        }
                    }
                }
                if(client.AssetLogoId != null || client.AssetProfileId != null)
                {
                    _clientRepository.UpdateEntity(client);
                }
                RegistrationCode regCode = new RegistrationCode();
                if(clientId != null && logoUploadSuccess && profileUploadSuccess 
                    && _registrationCodeRepository.Create(clientId.Value, out regCode) 
                    && _securityUserClientRepository.Create(client.CreateUserId.Value, client.Id))
                {
                    transaction.Complete();
                }
                return clientId;
            }
        }
        public int? UpdateClient(Client client, HttpPostedFileBase logo, HttpPostedFileBase profile)
        {
            int? resultingId = null;
            var existingClient = _clientRepository.GetById(client.Id);
            existingClient.UpdateDate = DateTime.Now;
            existingClient.Name = client.Name;
            existingClient.Description = client.Description;
            existingClient.DescriptionHtml = client.DescriptionHtml;
            existingClient.DescriptionJson = client.DescriptionJson;
            existingClient.Link = client.Link;
            //existingClient.AssetProfileID = client.AssetProfileID;
            //existingClient.AssetLogoID = client.AssetLogoID;
            existingClient.StreetAddress = client.StreetAddress;
            existingClient.Zipcode = client.Zipcode;
            existingClient.Phone = client.Phone;
            existingClient.SupportEmail = client.SupportEmail;
            existingClient.MarketingEmail = client.MarketingEmail;
            existingClient.City = client.City;
            existingClient.State = client.State;
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions()
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = CommonConfiguration.Configuration.TransactionScope_Timeout
            }))
            {
                var logoUploadSuccess = logo != null && string.IsNullOrWhiteSpace(logo.FileName);
                var baseLogoAssetId = existingClient.AssetLogoId;
                var deletingCurrentLogo = logoUploadSuccess && existingClient.AssetLogoId != null;
                if (deletingCurrentLogo)
                {
                    existingClient.AssetLogoId = null;
                    logoUploadSuccess = true;
                }
                else if (!logoUploadSuccess)
                {
                    existingClient.AssetLogoId = ProcessFileUpload(existingClient.Id, existingClient.CreateUserId.Value, existingClient.AssetLogoId, logo);
                    if (baseLogoAssetId != existingClient.AssetLogoId)
                    {
                        logoUploadSuccess = true;
                    }
                }

                var profileUploadSuccess = profile != null && string.IsNullOrWhiteSpace(profile.FileName);
                var baseProfileAssetId = existingClient.AssetProfileId;
                var deletingCurrentProfile = profileUploadSuccess && existingClient.AssetProfileId != null;
                if (deletingCurrentProfile)
                {
                    existingClient.AssetProfileId = null;
                    profileUploadSuccess = true;
                }
                else if (!profileUploadSuccess)
                {
                    existingClient.AssetProfileId = ProcessFileUpload(existingClient.Id, existingClient.CreateUserId.Value, existingClient.AssetProfileId, profile);
                    if (baseProfileAssetId != existingClient.AssetProfileId)
                    {
                        profileUploadSuccess = true;
                    }
                }
                resultingId = _clientRepository.UpdateEntity(existingClient);
                if (logoUploadSuccess && profileUploadSuccess && resultingId != null)
                {
                    transaction.Complete();
                }
            }
            return resultingId;
        }
        public int? CreateLocation(Location location, HttpPostedFileBase locationImage)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions()
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = CommonConfiguration.Configuration.TransactionScope_Timeout
            }))
            {
                var locationId = _locationRepository.Create(location);
                var baseLogoAssetId = location.AssetLocationId;
                // need to have identity service add claim for location to users cookie.
                var locationImageUploadSuccess = locationImage != null && string.IsNullOrWhiteSpace(locationImage.FileName);
                if (!locationImageUploadSuccess)
                {
                    location.AssetLocationId = ProcessFileUpload(location.ClientId, location.CreateUserId.Value, location.AssetLocationId, locationImage);
                    if (baseLogoAssetId != location.AssetLocationId)
                    {
                        locationImageUploadSuccess = true;
                        _locationRepository.UpdateEntity(location);
                    }
                }
                if (locationImageUploadSuccess && location.Id > 0)
                {
                    transaction.Complete();
                }
            }
            return location.Id;
        }
        public int? UpdateLocation(Location location, HttpPostedFileBase locationImage)
        {
            int? resultingId = null;
            var existingLocation = _locationRepository.GetById(location.Id);
            existingLocation.UpdateDate = DateTime.Now;
            existingLocation.Name = location.Name;
            existingLocation.Description = location.Description;
            existingLocation.DescriptionHtml = location.DescriptionHtml;
            existingLocation.DescriptionJson = location.DescriptionJson;
            existingLocation.StreetAddress = location.StreetAddress;
            existingLocation.Zipcode = location.Zipcode;
            existingLocation.City = location.City;
            existingLocation.State = location.State;
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions()
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = CommonConfiguration.Configuration.TransactionScope_Timeout
            }))
            {
                var locationImageUploadSuccess = locationImage != null && string.IsNullOrWhiteSpace(locationImage.FileName);
                var baseLogoAssetId = existingLocation.AssetLocationId;
                var deletingCurrentLogo = locationImageUploadSuccess && existingLocation.AssetLocationId != null;
                if (deletingCurrentLogo)
                {
                    existingLocation.AssetLocationId = null;
                    locationImageUploadSuccess = true;
                }
                else if (!locationImageUploadSuccess)
                {
                    existingLocation.AssetLocationId = ProcessFileUpload(existingLocation.ClientId, existingLocation.CreateUserId.Value, existingLocation.AssetLocationId, locationImage);
                    if (baseLogoAssetId != existingLocation.AssetLocationId)
                    {
                        locationImageUploadSuccess = true;
                    }
                }
                resultingId = _locationRepository.UpdateEntity(existingLocation);
                if (locationImageUploadSuccess && resultingId != null)
                {
                    transaction.Complete();
                }
            }
            return resultingId;
        }

        public string DeleteClient(Client client)
        {
            string errorMessage = "Failed to delete client";
            var children = _locationRepository.GetByClientId(client.Id);
            if (children.Count() == 0 && _clientRepository.DeleteEntity(client))
            {
                errorMessage = "";
            }
            else if (children.Count() > 0)
            {
                errorMessage = string.Format("Failed to delete client, locations associated with client must be deleted first.", children.Count());
            }
            return errorMessage;
        }
        public string DeleteLocation(Location location)
        {
            string errorMessage = "Failed to delete location";
            var children = _areaRepository.GetByLocationId(location.Id);
            if (children.Count() == 0 && _locationRepository.DeleteEntity(location))
            {
                errorMessage = "";
            }
            else if (children.Count() > 0)
            {
                errorMessage = string.Format("Failed to delete location, area associated with location must be deleted first.", children.Count());
            }
            return errorMessage;
        }

        public void CreateArea(Area area, HttpPostedFileBase areaMap)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions()
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = CommonConfiguration.Configuration.TransactionScope_Timeout
            }))
            {
                var areaId = _areaRepository.Create(area);

                var areaMapUploadSuccess = areaMap != null && string.IsNullOrWhiteSpace(areaMap.FileName);

                if (areaId != null)
                {
                    var baseLogoAssetId = area.AssetAreaId;
                    if (!areaMapUploadSuccess)
                    {
                        area.AssetAreaId = ProcessFileUpload(area.ClientId, area.CreateUserId.Value, area.AssetAreaId, areaMap);
                        if (baseLogoAssetId != area.AssetAreaId)
                        {
                            areaMapUploadSuccess = true;
                        }
                    }
                }
                if (area.AssetAreaId != null)
                {
                    _areaRepository.UpdateEntity(area);
                }
                if(area.Id > 0 && areaMapUploadSuccess)
                {
                    transaction.Complete();
                }
            }
        }

        public void UpdateArea(Area area, HttpPostedFileBase areaMap)
        {
            int? resultingId = null;
            var existingArea = _areaRepository.GetById(area.Id);
            existingArea.Description = area.Description;
            existingArea.DescriptionHtml = area.DescriptionHtml;
            existingArea.DescriptionJson = area.DescriptionJson;
            existingArea.Name = area.Name;
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions()
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = CommonConfiguration.Configuration.TransactionScope_Timeout
            }))
            {
                var areaMapUploadSuccess = string.IsNullOrWhiteSpace(areaMap.FileName);
                var baseLogoAssetId = existingArea.AssetAreaId;
                var deletingCurrentLogo = areaMapUploadSuccess && existingArea.AssetAreaId != null;
                if (deletingCurrentLogo)
                {
                    existingArea.AssetAreaId = null;
                    areaMapUploadSuccess = true;
                }
                else if (!areaMapUploadSuccess)
                {
                    existingArea.AssetAreaId = ProcessFileUpload(existingArea.ClientId, existingArea.CreateUserId.Value, existingArea.AssetAreaId, areaMap);
                    if (baseLogoAssetId != existingArea.AssetAreaId)
                    {
                        areaMapUploadSuccess = true;
                    }
                }
                resultingId = _areaRepository.UpdateEntity(existingArea);
                if (areaMapUploadSuccess && resultingId != null)
                {
                    transaction.Complete();
                }
            }
        }

        public string DeleteArea(Area area)
        {
            string errorMessage = "Failed to delete area";
            var childTours = _tourRepository.GetByAreaId(area.Id);
            if(childTours.Count() == 0 && _areaRepository.DeleteEntity(area))
            {
                errorMessage = "";
            }
            else if(childTours.Count() > 0)
            {
                errorMessage = string.Format("Failed to delete area, tours associated with area must be deleted first.", childTours.Count());
            }
            return errorMessage;
        }

        public void CreateTour(Tour tour, HttpPostedFileBase tourThumb, HttpPostedFileBase KrPanoZip)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions()
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = CommonConfiguration.Configuration.TransactionScope_Timeout
            }))
            {
                var tourId = _tourRepository.Create(tour);
                var tourThumbUploadSuccess = tourThumb != null && string.IsNullOrWhiteSpace(tourThumb.FileName);

                if (tourId != null)
                {
                    // upload tour thumb
                    var baseLogoAssetId = tour.AssetTourThumbnailId;
                    if (!tourThumbUploadSuccess)
                    {
                        tour.AssetTourThumbnailId = ProcessFileUpload(tour.ClientId, tour.CreateUserId.Value, tour.AssetTourThumbnailId, tourThumb);
                        if (baseLogoAssetId != tour.AssetTourThumbnailId)
                        {
                            tourThumbUploadSuccess = true;
                        }
                    }

                    var krPanoUploadSuccess = string.IsNullOrWhiteSpace(tourThumb.FileName);
                    if (!krPanoUploadSuccess)
                    {
                        // swap to krpano upload
                        tour.KrPanoTourId = UploadZipFolder(tour, tour.CreateUserId, KrPanoZip);
                        if (tour.KrPanoTourId != null)
                        {
                            krPanoUploadSuccess = true;
                        }
                    }
                }
                if (tour.AssetTourThumbnailId != null)
                {
                    _tourRepository.UpdateEntity(tour);
                }
                if (tour.Id > 0 && tourThumbUploadSuccess)
                {
                    transaction.Complete();
                }
            }
        }

        public void UpdateTour(Tour tour, HttpPostedFileBase tourThumb, HttpPostedFileBase KrPanoZip)
        {
            int? resultingId = null;
            var existingTour = _tourRepository.GetById(tour.Id);
            existingTour.Description = tour.Description;
            existingTour.DescriptionHtml = tour.DescriptionHtml;
            existingTour.DescriptionJson = tour.DescriptionJson;
            existingTour.Name = tour.Name;
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions()
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = CommonConfiguration.Configuration.TransactionScope_Timeout
            }))
            {
                var tourThumbUploadSuccess = string.IsNullOrWhiteSpace(tourThumb.FileName);
                var baseLogoAssetId = existingTour.AssetTourThumbnailId;
                var deletingCurrentTourThumb = tourThumbUploadSuccess && existingTour.AssetTourThumbnailId != null && tour.AssetTourThumbnailId == null;
                if (deletingCurrentTourThumb)
                {
                    existingTour.AssetTourThumbnailId = null;
                    tourThumbUploadSuccess = true;
                }
                else if (!tourThumbUploadSuccess)
                {
                    existingTour.AssetTourThumbnailId = ProcessFileUpload(existingTour.ClientId, existingTour.CreateUserId.Value, existingTour.AssetTourThumbnailId, tourThumb);
                    if (baseLogoAssetId != existingTour.AssetTourThumbnailId)
                    {
                        tourThumbUploadSuccess = true;
                    }
                }
                var krPanoUploadSuccess = string.IsNullOrWhiteSpace(KrPanoZip.FileName);
                var baseKrPanoAssetId = existingTour.KrPanoTourId;
                var deletingCurrentKrPano = krPanoUploadSuccess && existingTour.KrPanoTourId != null && tour.KrPanoTourId == null;
                if (deletingCurrentKrPano)
                {
                    existingTour.KrPanoTourId = null;
                    krPanoUploadSuccess = true;
                }
                else if (!krPanoUploadSuccess)
                {
                    // swap to krpano upload
                    existingTour.KrPanoTourId = UploadZipFolder(existingTour, tour.UpdateUserId, KrPanoZip);
                    if (baseKrPanoAssetId != existingTour.KrPanoTourId)
                    {
                        krPanoUploadSuccess = true;
                    }
                }
                resultingId = _tourRepository.UpdateEntity(existingTour);
                if (tourThumbUploadSuccess && krPanoUploadSuccess && resultingId != null)
                {
                    transaction.Complete();
                }
            }
        }

        private int? UploadZipFolder(Tour existingTour, int? createUserId, HttpPostedFileBase krPanoZip)
        {
            int? assetId = null;
            var clientId = existingTour.ClientId;
            var locationId = _areaRepository.GetById(existingTour.AreaId)?.Id;
            var areaId = existingTour.AreaId;
            var tourId = existingTour.Id;
            if (locationId != null)
            {
                MemoryStream target = new MemoryStream();
                krPanoZip.InputStream.CopyTo(target);
                byte[] data = target.ToArray();
                string path = _fileService.UploadZip(data, clientId, tourId, areaId, locationId.Value);
                var krPanoAsset = new AssetStore
                {
                    ClientId = clientId,
                    Filename = string.Format("KrPanoTour-{0}-{1}-{2}-{3}",clientId, locationId.Value, areaId, tourId),
                    FileType = "html",
                    Nickname = "krPano",
                    Path = path,
                    CreateDate = DateTime.Now,
                    CreateUserId = createUserId.Value
                };
                assetId = _assetStoreRepository.Create(krPanoAsset);
            }
            return assetId;
        }

        public string DeleteTour(Tour tour)
        {
            _tourRepository.DeleteEntity(tour);
            return "";
        }

        private int? ProcessFileUpload(int clientId, int userId, int? existingFilestoreId, HttpPostedFileBase file)
        {
            if(file != null && !string.IsNullOrWhiteSpace(file.FileName))
            {
                var assetStore = UploadAsset(file, userId, clientId, file.FileName);
                var assetId = _assetStoreRepository.Create(assetStore);
                return assetId;
            }
            return existingFilestoreId;
        }
        private int? ProcessFileUpload(int clientId, int userId, int? existingFilestoreId, string path, HttpPostedFileBase file)
        {
            if (file != null && !string.IsNullOrWhiteSpace(file.FileName))
            {
                var assetStore = UploadAsset(file, userId, clientId, file.FileName);
                var assetId = _assetStoreRepository.Create(assetStore);
                return assetId;
            }
            return existingFilestoreId;
        }
        protected AssetStore UploadAsset(HttpPostedFileBase file, int userId, int clientID, string fileNickname)
        {
            MemoryStream target = new MemoryStream();
            file.InputStream.CopyTo(target);
            byte[] data = target.ToArray();
            string path = string.Format("{0}/images/{1}", clientID, GuidUtils.AppendGuidToFilename(file.FileName));
            _fileService.UploadFile(data, path);
            return new AssetStore
            {
                ClientId = clientID,
                Filename = file.FileName,
                FileType = file.ContentType,
                Nickname = fileNickname,
                Path = path,
                CreateDate = DateTime.Now,
                CreateUserId = userId
            };
        }
        protected AssetStore CreateAsset(HttpPostedFileBase file, string path, int userId, int clientID, string fileNickname)
        {
            return new AssetStore
            {
                ClientId = clientID,
                Filename = file.FileName,
                FileType = file.ContentType,
                Nickname = fileNickname,
                Path = path,
                CreateDate = DateTime.Now,
                CreateUserId = userId
            };
        }
        internal static void SendRegistrationEmail(RegistrationCode registrationCode, string emailAddress)
        {
            string smtpAddress = "smtp.mail.yahoo.com";
            int portNumber = 587;
            bool enableSSL = true;

            string emailFrom = "VTCorePostalBot@yahoo.com";
            string password = @"6u\67\8[6+Cz2_9*Y]7DvTnG3EJ9#k@}";
            string emailTo = emailAddress;
            string subject = "Virtual Tour Registration Code";
            string body = File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/Views/Shared/EmailInvitation.htm"));
            //string body = string.Format("Hey, use this code ({0}) to register new users for your account.  This code will expire at {1}", registrationCode.Guid, ((DateTime)registrationCode.CreateDate).AddHours(1));
            body = body.Replace("#RegistrationCodeValue#", registrationCode.Guid.ToString());
            body = body.Replace("#ExpiryDateTimeValue#", ((DateTime)registrationCode.CreateDate).AddHours(1).ToString());


            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(emailFrom);
                mail.To.Add(emailTo);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                // Can set to false, if you are sending pure text.

                //mail.Attachments.Add(new Attachment("C:\\SomeFile.txt"));
                //mail.Attachments.Add(new Attachment("C:\\SomeZip.zip"));

                using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                {
                    smtp.Credentials = new System.Net.NetworkCredential(emailFrom, password);
                    smtp.EnableSsl = enableSSL;
                    smtp.Send(mail);
                }
            }
        }

        public void UpdateTourLocations(List<Tour> tours, int userId)
        {
            foreach(var tour in tours)
            {
                var existingTour = _tourRepository.GetById(tour.Id);
                if(existingTour != null)
                {
                    existingTour.MapX = tour.MapX;
                    existingTour.MapY = tour.MapY;
                    existingTour.UpdateUserId = userId;
                    existingTour.UpdateDate = DateTime.Now;
                    _tourRepository.UpdateEntity(existingTour);
                }
            }
        }

        public string IssueInvitation(int id, string email)
        {
            RegistrationCode regCode = null;
            string result = "Failed to complete.  Contact Support if issue continues.";
            if(_registrationCodeRepository.Create(id, out regCode))
            {
                SendRegistrationEmail(regCode, email);
                result = "";
            }
            return result;

        }
    }
}
