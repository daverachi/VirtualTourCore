using VirtualTourCore.Common.DataAccess.Interfaces;
using VirtualTourCore.Common.Logging;

namespace VirtualTourCore.Common.DataAccess
{
    public class Repository<T> : BaseRepository<T>, IRepository<T> where T : EntityBase
    {
        public Repository(IUnitOfWork uow, INlogger log) : base(uow, log)
        {
        }
    }
}
