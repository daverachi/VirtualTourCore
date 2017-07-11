using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualTourCore.Common.DataAccess;

namespace VirtualTourCore.Core.Models
{
    public class RegistrationCode : EntityBase
    {
        public RegistrationCode(int? clientId = null)
        {
            Guid = Guid.NewGuid();
            AvailableUsages = 1;
            ClientId = clientId;
        }
        public RegistrationCode() { }
        public Guid Guid { get; set; }
        public int AvailableUsages { get; set; }
        public int? ClientId { get; set; }
        [NotMapped]
        public string ErrorMessage { get; set; }
        [NotMapped]
        public bool Success
        {
            get
            {
                return string.IsNullOrWhiteSpace(ErrorMessage);
            }
        }
    }
}
