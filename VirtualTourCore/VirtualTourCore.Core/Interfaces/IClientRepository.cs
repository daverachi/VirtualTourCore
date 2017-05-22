using System;
using System.Collections.Generic;
using VirtualTourCore.Common.DataAccess.Interfaces;
using VirtualTourCore.Core.Models;

namespace VirtualTourCore.Core.Interfaces
{
    public interface IClientRepository : IBaseRepository<Client>
    {
        IEnumerable<Client> Get();
        IEnumerable<Client> GetByGuids(IEnumerable<Guid> guids);
        Client GetByGuid(Guid guid);
    }
}
