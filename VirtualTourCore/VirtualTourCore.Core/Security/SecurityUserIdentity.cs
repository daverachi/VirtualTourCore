using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace VirtualTourCore.Core.Security
{
    public class SecurityUserIdentity : GenericIdentity
    {
        public string[] UserData { get; set; }
        public SecurityUserIdentity(string a, string b) : base(a,b)
        {

        }
    }
}
