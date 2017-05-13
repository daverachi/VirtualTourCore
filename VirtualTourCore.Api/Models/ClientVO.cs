using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtualTourCore.Core.Models;

namespace VirtualTourCore.Api.Models
{
    public class ClientVO
    {
        public Client Client { get; set; }
        public bool IsModifiable { get; set; }
    }
}