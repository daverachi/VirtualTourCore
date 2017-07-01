using System;
using System.IO;
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

        public AdminService(
            IClientRepository clientRepository,
            IRegistrationCodeRepository registrationCodeRepository,
            ILocationRepository locationRepository,
            IAreaRepository areaRepository,
            ITourRepository tourRepository,
            IAssetStoreRepository assetStoreRepository,
            IFileService fileService
            )
        {
            _clientRepository = clientRepository;
            _registrationCodeRepository = registrationCodeRepository;
            _locationRepository = locationRepository;
            _areaRepository = areaRepository;
            _tourRepository = tourRepository;
            _assetStoreRepository = assetStoreRepository;
            _fileService = fileService;
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
                if (clientId != null && _registrationCodeRepository.Create(clientId.Value, out regCode))
                {
                    SendRegistrationEmail(regCode);
                }
                if(clientId != null && logoUploadSuccess && profileUploadSuccess)
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
                var logoUploadSuccess = string.IsNullOrWhiteSpace(logo.FileName);
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

                var profileUploadSuccess = string.IsNullOrWhiteSpace(profile.FileName);
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
        public int? CreateLocation(Location location)
        {
            var locationId = _locationRepository.Create(location);
            // need to have identity service add claim for location to users cookie.
            return locationId;
        }
        public int? UpdateLocation(Location location)
        {
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
            return _locationRepository.UpdateEntity(existingLocation);
        }

        public bool DeleteClient(Client client)
        {
            return _clientRepository.DeleteEntity(client);
        }
        public bool DeleteLocation(Location location)
        {
            return _locationRepository.DeleteEntity(location);
        }

        internal static void SendRegistrationEmail(RegistrationCode registrationCode)
        {
            string smtpAddress = "smtp.mail.yahoo.com";
            int portNumber = 587;
            bool enableSSL = true;

            string emailFrom = "VTCorePostalBot@yahoo.com";
            string password = @"6u\67\8[6+Cz2_9*Y]7DvTnG3EJ9#k@}";
            string emailTo = "david.ruhlemann@tallan.com";
            string subject = "Virtual Tour Registration Code";
            string body = string.Format("Hey, use this code ({0}) to register new users for your account.  This code will expire at {1}", registrationCode.Guid, ((DateTime)registrationCode.CreateDate).AddHours(1));

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

        public void CreateArea(Area area)
        {
            _areaRepository.Create(area);
        }

        public void UpdateArea(Area area)
        {
            var existingArea = _areaRepository.GetById(area.Id);
            existingArea.Description = area.Description;
            existingArea.DescriptionHtml = area.DescriptionHtml;
            existingArea.DescriptionJson = area.DescriptionJson;
            existingArea.Name = area.Name;
            _areaRepository.UpdateEntity(existingArea);
        }

        public void DeleteArea(Area area)
        {
            _areaRepository.DeleteEntity(area);
        }

        public void CreateTour(Tour tour)
        {
            _tourRepository.Create(tour);
        }

        public void UpdateTour(Tour tour)
        {
            var existingTour = _tourRepository.GetById(tour.Id);
            existingTour.Description = tour.Description;
            existingTour.DescriptionHtml = tour.DescriptionHtml;
            existingTour.DescriptionJson = tour.DescriptionJson;
            existingTour.Name = tour.Name;
            _tourRepository.UpdateEntity(existingTour);
        }

        public void DeleteTour(Tour tour)
        {
            _tourRepository.DeleteEntity(tour);
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



    }
}
