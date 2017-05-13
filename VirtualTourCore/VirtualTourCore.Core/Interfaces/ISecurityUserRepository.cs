using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualTourCore.Core.Models;

namespace VirtualTourCore.Core.Interfaces
{
    public interface ISecurityUserRepository
    {
        IEnumerable<SecurityUser> Get();
        SecurityUser GetById(int id);
        SecurityUser GetByUsername(string username);
        int? Create(SecurityUser user);
        SecurityUser Update(SecurityUser user);
        int Delete(int id);
        SecurityUser GetByLogin(string username, string password);
    }
}
