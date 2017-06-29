using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualTourCore.Core.Models;

namespace VirtualTourCore.Core.Interfaces
{
    public interface IRegistrationCodeRepository
    {
        RegistrationCode GetByGuid(Guid guid);
        bool Create(int clientId, out RegistrationCode regCode);
        RegistrationCode Consume(string guid);
        RegistrationCode GetById(int id);
    }
}
