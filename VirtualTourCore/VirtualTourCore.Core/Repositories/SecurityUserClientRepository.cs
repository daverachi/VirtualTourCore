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
    public class SecurityUserClientRepository : BaseRepository<SecurityUserClient>, ISecurityUserClientRepository
    {
        private IUnitOfWork _UnitOfWork;
        private INlogger _log;

        public SecurityUserClientRepository(IUnitOfWork UnitOfWork, INlogger log)
            : base(UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
            _log = log;
        }

        public IEnumerable<SecurityUserClient> GetByUserId(int id)
        {
            var securityUserClients = GetQueryable().Where(x => x.SecurityUserId == id);
            return securityUserClients;
        }

        public bool Create(SecurityUserClient userClient)
        {
            bool created = false;
            base.Add(userClient);
            int unit = _UnitOfWork.SaveChanges();
            if (unit < 1 || userClient.Id <= 0)
            {
                _log.Warn("Failed to Create new RegCode", new ArgumentNullException());
            }
            else
            {
                created = true;
            }
            return created;
        }
    }
}
