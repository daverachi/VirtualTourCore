using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualTourCore.Common.DataAccess;
using VirtualTourCore.Common.DataAccess.Interfaces;
using VirtualTourCore.Common.Logging;
using VirtualTourCore.Core.Interfaces;
using VirtualTourCore.Core.Models;

namespace VirtualTourCore.Core.Repositories
{
    public class ClientRepository : BaseRepository<Client>, IClientRepository
    {
        private IUnitOfWork _UnitOfWork;
        private INlogger _log;

        public ClientRepository(IUnitOfWork UnitOfWork, INlogger log)
            : base(UnitOfWork, log)
        {
            _UnitOfWork = UnitOfWork;
            _log = log;
        }

        public Client GetByGuid(Guid guid)
        {
            var client = GetQueryable()
                .FirstOrDefault(x => x.Guid == guid);
            if (client == null)
            {
                _log.Warn("Repo: Failed to retrieve Client from Client Guids: ", new ArgumentException());
            }
            return client;
        }
        public IEnumerable<Client> GetByGuids(IEnumerable<Guid> guids)
        {
            var clients = GetQueryable()
                .Where(x => guids.Contains(x.Guid));
            if (clients == null || clients.Count() == 0)
            {
                _log.Warn("Repo: Failed to retrieve Client from Client Guids: ", new ArgumentException());
            }
            return clients;
        }
    }
}
