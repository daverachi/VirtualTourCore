using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtualTourCore.Core.Models;

namespace VirtualTourCore.Api.Models
{
    public class AreaMapLocationVM
    {
        public Area ParentArea { get; set; }
        public IEnumerable<Tour> Tours { get; set; }
    }
}