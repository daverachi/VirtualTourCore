using System.Collections;
using System.Collections.Generic;

namespace VirtualTourCore.Core.Models
{
    public partial class SecurityUser
    {
        public SecurityUser()
        {
            Clients = new List<SecurityUserClient>();
        }

        public int Id { get; set; }
        public int RegistrationCodeId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Admin { get; set; }
        public IEnumerable<SecurityUserClient> Clients { get; set; }
    }
}
