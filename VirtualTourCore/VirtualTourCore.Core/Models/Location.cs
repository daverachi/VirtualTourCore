using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
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
        public int? AssetLocationId { get; set; }
        public AssetStore AssetLocation { get; set; }
        public int? CustomizationId { get; set; }
        public Customization Customization { get; set; }
        public int? ItemStatusId { get; set; }
        public ItemStatus ItemStatus { get; set; }
        public IEnumerable<ItemStatus> ItemStatuses { get; set; }
        [NotMapped]
        public string ValidationMessage { get; set; }
    }
}
