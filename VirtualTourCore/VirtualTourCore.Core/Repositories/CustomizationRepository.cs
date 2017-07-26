using VirtualTourCore.Common.DataAccess;
using VirtualTourCore.Common.DataAccess.Interfaces;
using VirtualTourCore.Common.Logging;
using VirtualTourCore.Core.Interfaces;
using VirtualTourCore.Core.Models;

namespace VirtualTourCore.Core.Repositories
{
    public class CustomizationRepository : BaseRepository<Customization>, ICustomizationRepository
    {
        private IUnitOfWork _UnitOfWork;
        private INlogger _log;

        public CustomizationRepository(IUnitOfWork UnitOfWork, INlogger log)
            : base(UnitOfWork, log)
        {
            _UnitOfWork = UnitOfWork;
            _log = log;
        }
    }
}
