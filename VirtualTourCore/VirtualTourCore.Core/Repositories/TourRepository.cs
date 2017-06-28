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
    public class TourRepository : BaseRepository<Tour>, ITourRepository
    {
        private IUnitOfWork _UnitOfWork;
        private INlogger _log;

        public TourRepository(IUnitOfWork UnitOfWork, INlogger log)
            : base(UnitOfWork, log)
        {
            _UnitOfWork = UnitOfWork;
            _log = log;
        }

        public IEnumerable<Tour> GetByAreaId(int areaId)
        {
            return GetQueryable().Where(x => x.AreaId == areaId);
        }

        public Tour GetByIdAndClientId(int id, IEnumerable<int> validClientIds)
        {
            return GetQueryable().FirstOrDefault(x => validClientIds.Contains(x.ClientId) && x.Id == id);
        }
    }
}
