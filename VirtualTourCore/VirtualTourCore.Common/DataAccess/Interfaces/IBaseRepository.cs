using System.Collections.Generic;

namespace VirtualTourCore.Common.DataAccess.Interfaces
{
    public interface IBaseRepository<T> : IReadOnlyBaseRepository<T>
        where T : class
    {
        T Add(T entity);
        T Update(T entity);
        void Delete(T entity);
        IEnumerable<T> Get();
        void Delete(IEnumerable<T> entities);
        T GetById(int id);
        int? Create(T entity);
        int? UpdateEntity(T entity);
        bool DeleteEntity(T entity);
    }
}
