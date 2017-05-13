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
            : base(UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
            _log = log;
        }

        public IEnumerable<Client> Get()
        {
            var clients = GetQueryable();
                //.Include(x => x.AssetLogoID)
                //.Include(x => x.AssetProfileID)
                //.Include(x => x.ItemStatus);
            if (clients == null)
            {
                _log.Warn("Failed to retrieve clients ", new ArgumentException());
            }
            return clients;
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
        public Client GetById(int id)
        {
            var client = GetQueryable()
                .FirstOrDefault(m => m.Id == id);
            if (client == null)
            {
                _log.Warn("Repo: Failed to retrieve Client from Client Id: " + id, new ArgumentException());
            }
            return client;
        }

        public int? Create(Client client)
        {
            int? id = null;
            if (client != null)
            {
                //_log.Info("Creating Client with Client Code: " + client.ClientCode);
                base.Add(client);
                int unit = _UnitOfWork.SaveChanges();
                if (unit < 1 || client.Id <= 0)
                {
                    //_log.Warn("Failed to Create Client with Client Code: " + client.ClientCode, new ArgumentNullException());
                }
                else
                {
                    _log.Info("Created Client with Client ID " + client.Id);
                    id = client.Id;
                }
            }
            return id;
        }

        public new int? Update(Client client)
        {
            int? id = null;
            if (client != null)
            {
                _log.Info("Update Client: " + client.Name + " ID: " + client.Name);
                base.Update(client);
                int unit = _UnitOfWork.SaveChanges();
                if (unit < 1)
                {
                    _log.Warn("Failed to Update Client: " + client.Name + " ID: " + client.Id, new ArgumentNullException());
                }
                else
                {
                    id = client.Id;
                }
            }
            return id;
        }

        public int Delete(int id)
        {
            int deleteId = id;
            _log.Info("Deleting Client with ID: " + id);
            Client client = GetById(id);
            if (client != null)
            {
                base.Delete(client);
                int unit = _UnitOfWork.SaveChanges();
                if (unit < 1)
                {
                    deleteId = unit;
                    _log.Warn("Failed to Delete Client with ID: " + id, new ArgumentNullException());
                }
            }
            return id;
        }
    }
}
