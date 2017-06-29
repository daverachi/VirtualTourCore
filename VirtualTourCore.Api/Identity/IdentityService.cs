using System;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using VirtualTourCore.Api.Models;
using VirtualTourCore.Core.Models;

namespace VirtualTourCore.Api.Identity
{
    public static class IdentityService
    {
        internal static void AuthorizeUser(SecurityUser securityUser)
        {
            var identity = new ClaimsIdentity(new GenericIdentity(securityUser.UserName), BuildUserClaims(securityUser), "Forms", securityUser.UserName, securityUser.Admin ? "Admin" : "ClientUser");
            if (securityUser.Admin)
            {
                identity.AddClaim(new Claim(identity.RoleClaimType, "Admin"));
            }
            var claimsIdentity = new ClaimsPrincipal(identity);

            var authedClaimsIdentity = FederatedAuthentication.FederationConfiguration.IdentityConfiguration.ClaimsAuthenticationManager.Authenticate("VT_Admin_Core", claimsIdentity);
            var token = FederatedAuthentication.SessionAuthenticationModule.CreateSessionSecurityToken(authedClaimsIdentity, "TOKEN_ISSUER", DateTime.UtcNow, DateTime.UtcNow.AddHours(1), false);

            FederatedAuthentication.SessionAuthenticationModule.CookieHandler.RequireSsl = false; // TODO ENFORCE SSL FOR PRODUCTION ENVIRONMENTS

            FederatedAuthentication.SessionAuthenticationModule.AuthenticateSessionSecurityToken(token, true);
        }

        internal static IEnumerable<string> GetClientIdsFromClaim(IPrincipal user)
        {
            var claimsIdentity = (ClaimsIdentity)user.Identity;
            var clientIds = new List<string>();
            var userClientsClaim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == "Clients");
            if (userClientsClaim != null)
            {
                clientIds = userClientsClaim.Value.Split(' ').Select(x => x.Trim()).ToList();
            }
            return clientIds;
        }

        private static IEnumerable<Claim> BuildUserClaims(SecurityUser securityUser)
        {
            var claims = new List<Claim>();
            if (securityUser != null)
            {
                string clientList = string.Empty;
                foreach (var client in securityUser.Clients)
                {
                    clientList += client.ClientId + " ";
                }
                if (!string.IsNullOrWhiteSpace(clientList))
                {
                    claims.Add(new Claim("Clients", clientList));
                }
            }
            claims.Add(new Claim("User_Id", securityUser.Id.ToString()));
            claims.Add(new Claim("Username", securityUser.UserName));
            claims.Add(new Claim("User_FirstName", securityUser.FirstName));
            claims.Add(new Claim("User_LastName", securityUser.LastName));
            claims.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", string.Format("{0}, {1}", securityUser.LastName, securityUser.FirstName)));
            claims.Add(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "VirtualTourCore"));
            if (securityUser.Admin && securityUser.AdminRegCode != null)
            {
                claims.Add(new Claim("UserCode", securityUser.AdminRegCode.Guid.ToString()));
            }
            return claims;
        }

        internal static int GetUserIdFromClaim(IPrincipal user)
        {
            var claimsIdentity = (ClaimsIdentity)user.Identity;
            var userid = 0;
            var userIdClaim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == "User_Id");
            if(userIdClaim != null)
            {
                int.TryParse(userIdClaim.Value, out userid);
            }
            return userid;
        }

        public static string GetUserNameFromClaim(IPrincipal user)
        {
            var claimsIdentity = (ClaimsIdentity)user.Identity;
            var firstNameClaim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == "User_FirstName");
            return firstNameClaim != null ? firstNameClaim.Value : "Unknown"; 
        }

        public static string GetUserCodeFromClaim(IPrincipal user)
        {
            var claimsIdentity = (ClaimsIdentity)user.Identity;
            var userCodeClaim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == "UserCode");
            return userCodeClaim.Value;
        }

        internal static IEnumerable<ClientVO> SetClientAccess(this IEnumerable<Client> clients, IPrincipal user)
        {
            List<ClientVO> clientVOs = new List<ClientVO>();
            var clientIds = GetClientIdsFromClaim(user);
            if(!user.IsInRole("Admin"))
            {
                //remove clients that the user should not see
                clients = clients.Where(x => clientIds.Contains(x.Id.ToString()));
            }
            foreach(var client in clients)
            {
                clientVOs.Add(new ClientVO {
                    Client = client,
                    IsModifiable = clientIds.Contains(client.Id.ToString()) ? true : false
                });
            }
            return clientVOs;
        }
        
        //const string MachineKeyPurpose = "MyApp:Username:{0}";
        //const string Anonymous = "<anonymous>";

        //string GetMachineKeyPurpose(IPrincipal user)
        //{
        //    return String.Format(MachineKeyPurpose,
        //        user.Identity.IsAuthenticated ? user.Identity.Name : Anonymous);
        //}

        //string Protect(byte[] data)
        //{
        //    if (data == null || data.Length == 0) return null;
        //    var purpose = GetMachineKeyPurpose(Thread.CurrentPrincipal);
        //    var value = MachineKey.Protect(data, purpose);
        //    return Convert.ToBase64String(value);
        //}

        //byte[] Unprotect(string value)
        //{
        //    if (String.IsNullOrWhiteSpace(value)) return null;
        //    var purpose = GetMachineKeyPurpose(Thread.CurrentPrincipal);
        //    var bytes = Convert.FromBase64String(value);
        //    return MachineKey.Unprotect(bytes, purpose);
        //}
    }
}