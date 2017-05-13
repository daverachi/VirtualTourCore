using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using VirtualTourCore.Core.Models;

namespace VirtualTourCore.Core.Interfaces
{
    public interface ISecurityService
    {
        IEnumerable<Claim> GetAccessTokenClaimsBySecurityUserId(int securityUserId);
        SecurityUser GetSecurityUserByLogin(string userName, string password);
        bool CreateUser(SecurityUser securityUser, string registrationCode);
    }
}
