using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualTourCore.Common.DataAccess;

namespace VirtualTourCore.Core.Models
{
    public class SecurityUserClient : EntityBase
    {
        public int Id { get; set; }
        public int SecurityUserId { get; set; }
        public int ClientId { get; set; }
    }
}
