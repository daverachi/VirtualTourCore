using VirtualTourCore.Common.DataAccess;

namespace VirtualTourCore.Core.Models
{
    public class Area : EntityBase
    {
        public int ClientId { get; set; }
        public int LocationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DescriptionJson { get; set; }
        public string DescriptionHtml { get; set; }
    }
}
