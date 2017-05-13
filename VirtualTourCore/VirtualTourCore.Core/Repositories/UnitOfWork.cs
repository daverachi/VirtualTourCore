using VirtualTourCore.Common.DataAccess;
using VirtualTourCore.Common.DataAccess.Interfaces;
using VirtualTourCore.Core.Interfaces;

namespace VirtualTourCore.Core.Repositories
{
    public class UnitOfWork : UnitOfWorkBase, IUnitOfWork
    {
        public UnitOfWork(IVirtualTourCoreContext context) //inject other contexts here
        {
            _DbContexts.Add(context);
        }
    }
}
