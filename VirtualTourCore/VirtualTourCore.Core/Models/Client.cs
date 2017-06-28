using System;
using VirtualTourCore.Common.DataAccess;
using VirtualTourCore.Core.Models.Base;

namespace VirtualTourCore.Core.Models
{
    public partial class Client : EntityBase
    {
        public string Name { get; set; }
        public Guid Guid { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public string DescriptionHtml { get; set; }
        public string DescriptionJson { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public string Phone { get; set; }
        public string SupportEmail { get; set; }
        public string MarketingEmail { get; set; }
        public int? AssetLogoId { get; set; }
        public int? AssetProfileId { get; set; }
    }
}
