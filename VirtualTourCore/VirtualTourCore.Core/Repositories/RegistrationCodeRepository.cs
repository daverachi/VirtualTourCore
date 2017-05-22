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
    public class RegistrationCodeRepository : BaseRepository<RegistrationCode>, IRegistrationCodeRepository
    {
        private IUnitOfWork _UnitOfWork;
        private INlogger _log;

        public RegistrationCodeRepository(IUnitOfWork UnitOfWork, INlogger log)
            : base(UnitOfWork, log)
        {
            _UnitOfWork = UnitOfWork;
            _log = log;
        }

        public RegistrationCode GetByGuid(Guid guid)
        {
            var regCode = GetQueryable().FirstOrDefault(m => m.Guid.ToString() == guid.ToString() && m.AvailableUsages > 0);
            if (regCode == null)
            {
                _log.Warn("Failed to retrieve a usable Reg Code from GUID: " + guid, new ArgumentException());
            }
            return regCode;
        }

        public bool Create(int clientId, out RegistrationCode regCode)
        {
            bool created = false;
            regCode = new RegistrationCode(clientId);
            base.Add(regCode);
            int unit = _UnitOfWork.SaveChanges();
            if (unit < 1 || regCode.Id <= 0)
            {
                _log.Warn("Failed to Create new RegCode", new ArgumentNullException());
            }
            else
            {
                created = true;
            }
            return created;
        }

        public RegistrationCode Consume(string guid)
        {
            Guid _guid;
            RegistrationCode regCode = null;
            if (Guid.TryParse(guid, out _guid))
            {
                regCode = GetByGuid(Guid.Parse(guid));
                if (regCode != null && regCode.AvailableUsages > 0 && regCode.CreateDate.Value.AddDays(2) > DateTime.Now)
                {
                    regCode.AvailableUsages--;
                    base.Update(regCode);
                    int unit = _UnitOfWork.SaveChanges();
                    if (unit < 1)
                    {
                        _log.Warn(string.Format("Failed to Consume reg code ID:{0} GUID:{1}", regCode.Id, regCode.Guid), new ArgumentNullException());
                    }
                }
            }
            return regCode;
        }
    }
}
