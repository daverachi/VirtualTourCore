using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualTourCore.Core.Models;

namespace VirtualTourCore.Core.Interfaces
{
    public interface ISecurityUserClientRepository
    {
        IEnumerable<SecurityUserClient> GetByUserId(int id);
        bool Create(SecurityUserClient userClient);
        bool Create(int userId, int clientId);
    }
}
