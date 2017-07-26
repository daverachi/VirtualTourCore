using System.ComponentModel.DataAnnotations.Schema;
using VirtualTourCore.Common.DataAccess;

namespace VirtualTourCore.Core.Models
{
    public class Customization : EntityBase
    {
        public string CustomCSS { get; set; }
        public string CustomJS { get; set; }
        [NotMapped]
        public string EntityName { get; set; }
    }
}
