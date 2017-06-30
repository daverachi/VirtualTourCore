using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtualTourCore.Common.ValueObjects
{
    public class NavigationBreadCrumbVO
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public int AreaId { get; set; }
        public string AreaName { get; set; }
        public int TourId { get; set; }
        public string TourName { get; set; }
    }
}