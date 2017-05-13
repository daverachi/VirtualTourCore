using VirtualTourCore.Common.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace VirtualTourCore.Common.DataAccess
{
    public abstract class BaseRepository<T> : ReadOnlyBaseRepository<T>, IBaseRepository<T>, IReadOnlyBaseRepository<T>
            where T : class
    {
        public BaseRepository(IUnitOfWork UoW)
            : base(UoW)
        {

        }

        public virtual T Add(T entity)
        {
            _Context.Set<T>().Add(entity);
            return entity;
        }

        public virtual T Update(T entity)
        {
            var entry = _Context.Entry<T>(entity);
            entry.State = EntityState.Modified;
            return entity;
        }

        public virtual void Delete(T entity)
        {
            _Context.Set<T>().Remove(entity);
        }

        public virtual void Delete(IEnumerable<T> entities)
        {
            _Context.Set<T>().RemoveRange(entities);
        }

        private bool EventTypeFilter(System.Reflection.PropertyInfo propertyInfo)
        {
            Type propertyType = propertyInfo.PropertyType;
            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(ICollection<>))
            {
                return false;
            }
            if (propertyType.Namespace.StartsWith("System"))
            {
                return true;
            }
            return false;
        }

        private object GetPropertyValue(object o)
        {
            if (o == null)
                return DBNull.Value;
            return o;
        }
    }
}
