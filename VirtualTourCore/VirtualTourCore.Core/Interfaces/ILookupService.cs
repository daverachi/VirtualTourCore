using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualTourCore.Core.Models;

namespace VirtualTourCore.Core.Interfaces
{
    public interface ILookupService
    {
        IEnumerable<Client> GetClientByGuids(IEnumerable<Guid> guids);
        Client GetClientByGuid(Guid guid);
        Client GetClientById(int id);
        IEnumerable<Client> GetClients();
    }
}
