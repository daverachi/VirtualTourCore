using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualTourCore.Common.DataAccess.Interfaces;
using VirtualTourCore.Core.Models;

namespace VirtualTourCore.Core.Interfaces
{
    public interface ILocationRepository : IBaseRepository<Location>
    {
        IEnumerable<Location> GetByClientId(int clientId);
        Location GetByIdFilteredByClientIds(int id, List<int> _validClientIds);
    }
}
