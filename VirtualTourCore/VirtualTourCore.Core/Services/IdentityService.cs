//using IdentityServer3.Core.Extensions;
//using IdentityServer3.Core.Models;
//using IdentityServer3.Core.Services;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;
//using VirtualTourCore.Core.Interfaces;
//using VirtualTourCore.Core.Models;

//namespace VirtualTourCore.Core.Services
//{
//    public class IdentityService : IUserService
//    {
//        private ISecurityService _securityService;

//        public IdentityService(ISecurityService securityService)
//        {
//            _securityService = securityService;
//        }

//        public Task AuthenticateExternalAsync(ExternalAuthenticationContext context)
//        {
//            return Task.FromResult<AuthenticateResult>(null);
//        }

//        public Task AuthenticateLocalAsync(LocalAuthenticationContext context)
//        {
//            Task<AuthenticateResult> authenticateResultAsync;

//            // assign subject to SecurityUserId and used for "sub" claimtype as identifier for user
//            if (string.IsNullOrWhiteSpace(context.UserName) || string.IsNullOrWhiteSpace(context.Password))
//            {
//                authenticateResultAsync = Task.FromResult<AuthenticateResult>(null);
//            }
//            else
//            {
//                SecurityUser subject = _securityService.GetSecurityUserByLogin(context.UserName, context.Password);

//                authenticateResultAsync =
//                    (subject == null)
//                    ? authenticateResultAsync = Task.FromResult<AuthenticateResult>(null)
//                    : authenticateResultAsync = Task.FromResult(new AuthenticateResult(subject.ToString(), context.UserName));
//            }

//            return authenticateResultAsync;
//        }

//        public Task GetProfileDataAsync(ProfileDataRequestContext context)
//        {
//            Task<IEnumerable<Claim>> claimsAsync;

//            if (context.Subject == null)
//            {
//                claimsAsync = Task.FromResult<IEnumerable<Claim>>(null);
//            }
//            else
//            {
//                // builds and associates list of abilities and companies for each user
//                int securityUserId;
//                bool isSubjectIdValid = Int32.TryParse(context.Subject.GetSubjectId(), out securityUserId);
//                if (isSubjectIdValid)
//                {
//                    IEnumerable<Claim> claims = _securityService.GetAccessTokenClaimsBySecurityUserId(securityUserId);
//                    claimsAsync =
//                        (claims == null || !claims.Any())
//                        ? Task.FromResult<IEnumerable<Claim>>(null)
//                        : Task.FromResult(claims);
//                }
//                else
//                {
//                    claimsAsync = Task.FromResult<IEnumerable<Claim>>(null);
//                }
//            }

//            return claimsAsync;
//        }

//        public Task IsActiveAsync(IsActiveContext context)
//        {
//            return Task.FromResult(true);
//        }

//        public Task PostAuthenticateAsync(PostAuthenticationContext context)
//        {
//            return Task.FromResult<AuthenticateResult>(null);
//        }

//        public Task PreAuthenticateAsync(PreAuthenticationContext context)
//        {
//            return Task.FromResult<AuthenticateResult>(null);
//        }

//        public Task SignOutAsync(SignOutContext context)
//        {
//            return Task.FromResult(0);
//        }
//    }
//}
