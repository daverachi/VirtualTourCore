using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualTourCore.Common.ValueObjects
{
    public class RegisterDTO
    {
        public string RegistrationCode { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
    }
}
