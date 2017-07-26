using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using VirtualTourCore.Common.DataAccess;
using VirtualTourCore.Core.Interfaces;

namespace VirtualTourCore.Core.Models
{
    public class Area : EntityBase, INavigableEntity
    {
        public Area()
        {
            ItemStatuses = new List<SelectListItem>();
        }
        public int ClientId { get; set; }
        public int LocationId { get; set; }
        [DisplayName("Area Name")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string DescriptionJson { get; set; }
        [AllowHtml]
        public string DescriptionHtml { get; set; }
        public int? AssetAreaId { get; set; }
        public AssetStore AssetArea { get; set; }
        public int? CustomizationId { get; set; }
        public Customization Customization { get; set; }
        public int? ItemStatusId { get; set; }
        public ItemStatus ItemStatus { get; set; }
        public IEnumerable<SelectListItem> ItemStatuses { get; set; }
        [NotMapped]
        public string ValidationMessage { get; set; }
    }
}
