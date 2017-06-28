using System.Collections.Generic;
using VirtualTourCore.Common.DataAccess.Interfaces;
using VirtualTourCore.Core.Models;

namespace VirtualTourCore.Core.Interfaces
{
    public interface IAreaRepository : IBaseRepository<Area>
    {
        IEnumerable<Area> GetByClientId(int id);
        Area GetByIdAndByClientId(int id, IEnumerable<int> enumerable);
    }
}
