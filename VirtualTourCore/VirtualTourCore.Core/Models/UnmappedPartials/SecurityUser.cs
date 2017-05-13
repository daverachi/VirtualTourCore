using System.ComponentModel.DataAnnotations.Schema;

namespace VirtualTourCore.Core.Models
{
    public partial class SecurityUser
    {
        [NotMapped]
        public string PasswordPlaintext { get; set; }
        [NotMapped]
        public string PasswordPlaintextConfirm { get; set; }
    }
}
