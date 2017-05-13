using VirtualTourCore.Common.DataAccess.Interfaces;

namespace VirtualTourCore.Common.DataAccess
{
    public class Repository<T> : BaseRepository<T>, IRepository<T> where T : class
    {
        public Repository(IUnitOfWork uow) : base(uow)
        {
        }
    }
}
