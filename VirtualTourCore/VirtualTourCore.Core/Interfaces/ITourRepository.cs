using System.Collections.Generic;
using VirtualTourCore.Common.DataAccess.Interfaces;
using VirtualTourCore.Core.Models;

namespace VirtualTourCore.Core.Interfaces
{
    public interface ITourRepository : IBaseRepository<Tour>
    {
        IEnumerable<Tour> GetByAreaId(int areaId);
        Tour GetByIdAndClientId(int id, IEnumerable<int> enumerable);
    }
}
