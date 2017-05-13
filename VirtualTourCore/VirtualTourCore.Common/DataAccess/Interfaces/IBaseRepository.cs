using System.Collections.Generic;

namespace VirtualTourCore.Common.DataAccess.Interfaces
{
    public interface IBaseRepository<T> : IReadOnlyBaseRepository<T>
        where T : class
    {
        T Add(T entity);
        T Update(T entity);
        void Delete(T entity);
        void Delete(IEnumerable<T> entities);
    }
}
