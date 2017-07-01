using System.ComponentModel;
using System.Web.Mvc;
using VirtualTourCore.Common.DataAccess;
using VirtualTourCore.Core.Interfaces;

namespace VirtualTourCore.Core.Models
{
    public class Tour : EntityBase, INavigableEntity
    {
        public int ClientId { get; set; }
        public int AreaId { get; set; }
        [DisplayName("Tour Name")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string DescriptionJson { get; set; }
        [AllowHtml]
        public string DescriptionHtml { get; set; }
        public int? AssetTourThumbnailId { get; set; }
        public AssetStore AssetTourThumbnail { get; set; }
        public int? KrPanoTourId { get; set; }
        public AssetStore KrPanoTour { get; set; }
    }
}
