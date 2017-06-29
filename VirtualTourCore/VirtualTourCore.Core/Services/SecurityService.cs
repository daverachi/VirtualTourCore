using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using VirtualTourCore.Common.Helper;
using VirtualTourCore.Core.Interfaces;
using VirtualTourCore.Core.Models;

namespace VirtualTourCore.Core.Services
{
    public class SecurityService : ISecurityService
    {
        private IRegistrationCodeRepository _registrationCodeRepository;
        private ISecurityUserRepository _securityUserRepository;
        private ISecurityUserClientRepository _securityUserClientRepository;
        public SecurityService(
            IRegistrationCodeRepository registrationCodeRepository,
            ISecurityUserRepository securityUserRepository,
            ISecurityUserClientRepository securityUserClientRepository)
        {
            _registrationCodeRepository = registrationCodeRepository;
            _securityUserRepository = securityUserRepository;
            _securityUserClientRepository = securityUserClientRepository;
        }

        //public bool SendRegistrationLink(string emailAddress)
        //{
        //    // this will be where we will create an email with a link to register.
        //    return GenerateNewRegistrationCode();
        //}

        //private bool GenerateNewRegistrationCode()
        //{
        //    return _registrationCodeRepository.Create();
        //}

        public SecurityUser GetSecurityUserByLogin(string username, string password)
        {
            SecurityUser user = null;
            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                user = _securityUserRepository.GetByLogin(username, password);
            }
            if (user != null)
            {
                if (user.Admin)
                {
                    user.AdminRegCode = _registrationCodeRepository.GetById(user.RegistrationCodeId);
                }
                user.Clients = _securityUserClientRepository.GetByUserId(user.Id);
                //_log.Warn("Failed to retrieve SecurityUser with login credentials");
            }

            return user;
        }


        public IEnumerable<Claim> GetAccessTokenClaimsBySecurityUserId(int securityUserId)
        {
            throw new NotImplementedException();
        }

        public bool CreateUser(SecurityUser user, string registrationCode)
        {
            bool created = false;
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions()
            {
                IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
                Timeout = CommonConfiguration.Configuration.TransactionScope_Timeout
            }))
            {
                var regCode = _registrationCodeRepository.Consume(registrationCode);

                if (regCode.Success)
                {
                    user.RegistrationCodeId = regCode.Id;
                    var userid = _securityUserRepository.Create(user);
                    if (regCode.ClientId != null)
                    {
                        var userClient = new SecurityUserClient
                        {
                            ClientId = regCode.ClientId.Value,
                            CreateUserId = userid,
                            SecurityUserId = (int)userid,
                        };
                        if(_securityUserClientRepository.Create(userClient))
                        {
                            user.Clients = new List<SecurityUserClient> { userClient };
                        }
                    }
                    if(userid != null && userid > 0 
                        && (regCode.ClientId == null || user.Clients.Count() > 0))
                    {
                        transaction.Complete();
                        created = true;
                    }
                }
                return created;
            }
        }
    }
}
