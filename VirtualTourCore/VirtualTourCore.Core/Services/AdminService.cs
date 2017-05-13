using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using VirtualTourCore.Core.Interfaces;
using VirtualTourCore.Core.Models;

namespace VirtualTourCore.Core.Services
{
    public class AdminService : IAdminService
    {
        private IClientRepository _clientRepository;
        private IRegistrationCodeRepository _registrationCodeRepository;
        public AdminService(
            IClientRepository clientRepository,
            IRegistrationCodeRepository registrationCodeRepository
            )
        {
            _clientRepository = clientRepository;
            _registrationCodeRepository = registrationCodeRepository;
        }
        public int? CreateClient(Client client)
        {
            client.Guid = Guid.NewGuid();
            var clientId = _clientRepository.Create(client);
            RegistrationCode regCode = new RegistrationCode();
            if (clientId != null && _registrationCodeRepository.Create(clientId.Value, out regCode))
            {
                SendRegistrationEmail(regCode);
            }
            return clientId;
        }
        public int? UpdateClient(Client client)
        {
            var existingClient = _clientRepository.GetById(client.Id);
            existingClient.UpdateDate = DateTime.Now;
            existingClient.Name = client.Name;
            existingClient.Guid = client.Guid;
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
            return _clientRepository.Update(existingClient);
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
    }
}
