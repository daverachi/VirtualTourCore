using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using VirtualTourCore.Core.Models;

namespace VirtualTourCore.Core.Security.Principal
{
    public class SecurityUserPrincipal : GenericPrincipal
    {
        public List<int> ClientIds { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public SecurityUserPrincipal(IIdentity identity, string[] roles, SecurityUser user) 
            : base(identity, roles)
        {
            UserId = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
        }
    }
}
