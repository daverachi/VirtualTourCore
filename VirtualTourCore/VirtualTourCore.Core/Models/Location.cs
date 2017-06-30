using System.ComponentModel;
using System.Web.Mvc;
using VirtualTourCore.Common.DataAccess;
using VirtualTourCore.Core.Interfaces;

namespace VirtualTourCore.Core.Models
{
    public class Location : EntityBase, INavigableEntity
    {
        public int ClientId { get; set; }
        [DisplayName("Location Name")]
        public string Name { get; set; }
        public string Description { get; set; }
        [AllowHtml]
        public string DescriptionHtml { get; set; }
        public string DescriptionJson { get; set; }
        [DisplayName("Street Address")]
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        [DisplayName("Zip Code")]
        public string Zipcode { get; set; }
    }
}
