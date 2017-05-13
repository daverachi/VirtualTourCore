using System;
using System.Collections.Generic;
using VirtualTourCore.Core.Models;

namespace VirtualTourCore.Core.Interfaces
{
    public interface IClientRepository
    {
        IEnumerable<Client> Get();
        IEnumerable<Client> GetByGuids(IEnumerable<Guid> guids);
        Client GetByGuid(Guid guid);
        Client GetById(int id);
        int? Create(Client client);
        int? Update(Client client);
        int Delete(int id);
    }
}
