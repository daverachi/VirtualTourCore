using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualTourCore.Common.DataAccess;
using VirtualTourCore.Common.DataAccess.Interfaces;
using VirtualTourCore.Common.Extensions;
using VirtualTourCore.Common.Logging;
using VirtualTourCore.Core.Interfaces;
using VirtualTourCore.Core.Models;

namespace VirtualTourCore.Core.Repositories
{
    public class LocationRepository : BaseRepository<Location>, ILocationRepository
    {
        private IUnitOfWork _UnitOfWork;
        private INlogger _log;

        public LocationRepository(IUnitOfWork UnitOfWork, INlogger log)
            : base(UnitOfWork, log)
        {
            _UnitOfWork = UnitOfWork;
            _log = log;
        }

        public IEnumerable<Location> GetByClientId(int clientId)
        {
            var locations = GetQueryable()
                .Where(x=>x.ClientId == clientId);
            if (locations == null)
            {
                _log.Warn("Failed to retrieve clients ", new ArgumentException());
            }
            return locations;
        }

        public Location GetByIdFilteredByClientIds(int id, IEnumerable<int> _validClientIds)
        {
            var location = GetQueryable()
               .FirstOrDefault(x => _validClientIds.Contains(x.ClientId) && x.Id == id);
            if (location == null)
            {
                _log.Error(string.Format("Failed to find Location with id: {0} associated to user with client credentials for ids: [{1}]", 
                    id, _validClientIds.ConvertIntListToCSV()), new ArgumentException());
            }
            return location;
        }
    }
}
