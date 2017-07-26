using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualTourCore.Common.DataAccess;

namespace VirtualTourCore.Core.Models
{
    public class Customization : EntityBase
    {
        public string CustomCSS { get; set; }
        public string CustomJS { get; set; }
    }
}
