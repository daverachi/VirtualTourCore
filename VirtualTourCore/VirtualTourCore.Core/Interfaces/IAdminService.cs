using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualTourCore.Core.Models;

namespace VirtualTourCore.Core.Interfaces
{
    public interface IAdminService
    {
        int? CreateClient(Client client);
        int? UpdateClient(Client client);
        int? CreateLocation(Location location);
        int? UpdateLocation(Location location);
        bool DeleteClient(Client client);
        bool DeleteLocation(Location location);
    }
}
