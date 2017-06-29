using System.Web.Mvc;
using VirtualTourCore.Common.DataAccess;

namespace VirtualTourCore.Core.Models
{
    public class Location : EntityBase
    {
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [AllowHtml]
        public string DescriptionHtml { get; set; }
        public string DescriptionJson { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
    }
}
