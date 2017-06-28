using System;
using System.Collections.Generic;
using System.Linq;
using VirtualTourCore.Common.DataAccess;
using VirtualTourCore.Common.DataAccess.Interfaces;
using VirtualTourCore.Common.Logging;
using VirtualTourCore.Core.Interfaces;
using VirtualTourCore.Core.Models;

namespace VirtualTourCore.Core.Repositories
{
    public class AreaRepository : BaseRepository<Area>, IAreaRepository
    {
        private IUnitOfWork _UnitOfWork;
        private INlogger _log;

        public AreaRepository(IUnitOfWork UnitOfWork, INlogger log)
            : base(UnitOfWork, log)
        {
            _UnitOfWork = UnitOfWork;
            _log = log;
        }

        public IEnumerable<Area> GetByClientId(int id)
        {
            var areas = GetQueryable().Where(x => x.ClientId == id);
            return areas;
        }

        public Area GetByIdAndByClientId(int id, IEnumerable<int> clientIds)
        {
            var area = GetQueryable().FirstOrDefault(x => clientIds.Contains(x.ClientId) && x.Id == id);
            return area;
        }

        public IEnumerable<Area> GetByLocationId(int id)
        {
            var areas = GetQueryable().Where(x => x.LocationId == id);
            return areas;
        }
    }
}
