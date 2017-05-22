using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualTourCore.Common.DataAccess;

namespace VirtualTourCore.Core.Models
{
    public class Location : EntityBase
    {
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DescriptionHtml { get; set; }
        public string DescriptionJson { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
    }
}
