using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using VirtualTourCore.Common.DataAccess;
using VirtualTourCore.Core.Interfaces;

namespace VirtualTourCore.Core.Models
{
    public partial class Client : EntityBase, INavigableEntity
    {
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
        public int? AssetProfileId { get; set; }
    }
}
