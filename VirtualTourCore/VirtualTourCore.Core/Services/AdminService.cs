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

        #region Client CRUD

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

                client.AssetLogoId = ProcessIncomingImage(client.Id, client.CreateUserId.Value, client.AssetLogoId, null, logo);
                client.AssetProfileId = ProcessIncomingImage(client.Id, client.CreateUserId.Value, client.AssetProfileId, null, profile);
                if (client.AssetLogoId != null || client.AssetProfileId != null)
                {
                    _clientRepository.UpdateEntity(client);
                }
                RegistrationCode regCode = new RegistrationCode();
                if(clientId != null && _registrationCodeRepository.Create(clientId.Value, out regCode) 
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
            existingClient.StreetAddress = client.StreetAddress;
            existingClient.Zipcode = client.Zipcode;
            existingClient.Phone = client.Phone;
            existingClient.SupportEmail = client.SupportEmail;
            existingClient.MarketingEmail = client.MarketingEmail;
            existingClient.City = client.City;
            existingClient.State = client.State;
            existingClient.ItemStatusId = client.ItemStatusId;
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions()
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = CommonConfiguration.Configuration.TransactionScope_Timeout
            }))
            {
                existingClient.AssetLogoId = ProcessIncomingImage(existingClient.Id, existingClient.CreateUserId.Value, client.AssetLogoId, existingClient.AssetLogoId, logo);
                existingClient.AssetProfileId = ProcessIncomingImage(existingClient.Id, existingClient.CreateUserId.Value, client.AssetProfileId, existingClient.AssetProfileId, profile);
                resultingId = _clientRepository.UpdateEntity(existingClient);
                if (resultingId != null)
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

        #endregion

        #region Location CRUD
        public int? CreateLocation(Location location, HttpPostedFileBase locationImage)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions()
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = CommonConfiguration.Configuration.TransactionScope_Timeout
            }))
            {
                var locationId = _locationRepository.Create(location);
                location.AssetLocationId = ProcessIncomingImage(location.ClientId, location.CreateUserId.Value, location.AssetLocationId, null, locationImage);
                if (location.AssetLocationId != null && _locationRepository.UpdateEntity(location) > 0 && location.Id > 0)
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
                existingLocation.AssetLocationId = ProcessIncomingImage(existingLocation.ClientId, 
                    existingLocation.CreateUserId.Value, location.AssetLocationId, existingLocation.AssetLocationId, locationImage);
                resultingId = _locationRepository.UpdateEntity(existingLocation);
                if (resultingId != null)
                {
                    transaction.Complete();
                }
            }
            return resultingId;
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

        #endregion

        #region Area CRUD
        public void CreateArea(Area area, HttpPostedFileBase areaMap)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions()
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = CommonConfiguration.Configuration.TransactionScope_Timeout
            }))
            {
                _areaRepository.Create(area);
                area.AssetAreaId = ProcessIncomingImage(area.ClientId, area.CreateUserId.Value, area.AssetAreaId, null, areaMap);
                if (area.AssetAreaId != null && _areaRepository.UpdateEntity(area) > 0 && area.Id > 0)
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
                existingArea.AssetAreaId = ProcessIncomingImage(existingArea.ClientId, 
                    existingArea.CreateUserId.Value, area.AssetAreaId, existingArea.AssetAreaId, areaMap);
                resultingId = _areaRepository.UpdateEntity(existingArea);
                if (resultingId != null)
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

        #endregion

        #region Tour CRUD
        public void CreateTour(Tour tour, HttpPostedFileBase tourThumb, HttpPostedFileBase KrPanoZip)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions()
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = CommonConfiguration.Configuration.TransactionScope_Timeout
            }))
            {
                var tourId = _tourRepository.Create(tour);
                tour.AssetTourThumbnailId = ProcessIncomingImage(tour.ClientId, tour.CreateUserId.Value,
                        tour.AssetTourThumbnailId, null, tourThumb);
                tour.KrPanoTourId = ProcessIncomingKrPano(tour, tour.KrPanoTourId, null, KrPanoZip);
                if (tour.AssetTourThumbnailId != null && _tourRepository.UpdateEntity(tour) > 0 && tour.Id > 0)
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
                existingTour.AssetTourThumbnailId = ProcessIncomingImage(existingTour.ClientId, existingTour.CreateUserId.Value, 
                    tour.AssetTourThumbnailId, existingTour.AssetTourThumbnailId, tourThumb);
                existingTour.KrPanoTourId = ProcessIncomingKrPano(existingTour, tour.KrPanoTourId, existingTour.KrPanoTourId, KrPanoZip);
                resultingId = _tourRepository.UpdateEntity(existingTour);
                if (resultingId != null)
                {
                    transaction.Complete();
                }
            }
        }
        public void UpdateTourLocations(List<Tour> tours, int userId)
        {
            foreach (var tour in tours)
            {
                var existingTour = _tourRepository.GetById(tour.Id);
                if (existingTour != null)
                {
                    existingTour.MapX = tour.MapX;
                    existingTour.MapY = tour.MapY;
                    existingTour.UpdateUserId = userId;
                    existingTour.UpdateDate = DateTime.Now;
                    _tourRepository.UpdateEntity(existingTour);
                }
            }
        }

        public string DeleteTour(Tour tour)
        {
            _tourRepository.DeleteEntity(tour);
            return "";
        }

        #endregion

        #region File Management
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

        private int? ProcessIncomingKrPano(Tour tour, int? formId, int? existingFilestoreId, HttpPostedFileBase file)
        {
            bool hadFile = existingFilestoreId != null;
            bool uploadedFile = (file != null && file.ContentLength != 0);
            bool noChangeToFile = formId == existingFilestoreId && !uploadedFile;
            bool deletingFile = hadFile && !noChangeToFile;

            if (uploadedFile)
            {
                existingFilestoreId = UploadZipFolder(tour, tour.CreateUserId, file);
            }
            else if (deletingFile)
            {
                existingFilestoreId = null;
            }
            return existingFilestoreId;
        }

        private int? ProcessIncomingImage(int clientId, int userId, int? formId, int? existingFilestoreId, HttpPostedFileBase file)
        {
            bool hadFile = existingFilestoreId != null;
            bool uploadedFile = (file != null && file.ContentLength != 0);
            bool noChangeToFile = formId == existingFilestoreId && !uploadedFile;
            bool deletingFile = hadFile && !noChangeToFile;

            if (uploadedFile)
            {
                existingFilestoreId = ProcessFileUpload(clientId, userId, existingFilestoreId, file);
            }
            else if (deletingFile)
            {
                existingFilestoreId = null;
            }
            return existingFilestoreId;
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

        #endregion

        #region Registration Management
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
        #endregion
    }
}
