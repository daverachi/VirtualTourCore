using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using VirtualTourCore.Common.DataAccess;
using VirtualTourCore.Core.Interfaces;

namespace VirtualTourCore.Core.Models
{
    public partial class Client : EntityBase, INavigableEntity
    {
        public Client()
        {
            ItemStatuses = new List<SelectListItem>();
        }
        public string Name { get; set; }
        public Guid Guid { get; set; }
        [DisplayName("Homepage Link")]
        public string Link { get; set; }
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
        [DisplayName("Phone Number")]
        public string Phone { get; set; }
        [DisplayName("Support Email Address")]
        public string SupportEmail { get; set; }
        [DisplayName("Marketing Email Address")]
        public string MarketingEmail { get; set; }
        public int? AssetLogoId { get; set; }
        public AssetStore AssetLogo { get; set; }
        public int? AssetProfileId { get; set; }
        public AssetStore AssetProfile { get; set; }
        public int? CustomizationId { get; set; }
        public Customization Customization { get; set; }
        public int? ItemStatusId { get; set; }
        public ItemStatus ItemStatus { get; set; }
        public IEnumerable<SelectListItem> ItemStatuses { get; set; }
        [NotMapped]
        public string ValidationMessage { get; set; }
    }
}
