using VirtualTourCore.Common.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using VirtualTourCore.Common.Logging;

namespace VirtualTourCore.Common.DataAccess
{
    public abstract class BaseRepository<T> : ReadOnlyBaseRepository<T>, IBaseRepository<T>, IReadOnlyBaseRepository<T>
            where T : VTCoreEntity
    {
        private IUnitOfWork _UnitOfWork;
        private INlogger _log;

        public BaseRepository(IUnitOfWork UoW, INlogger log)
            : base(UoW)
        {
            _UnitOfWork = UnitOfWork;
            _log = log;
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
        public IEnumerable<T> Get()
        {
            var entities = GetQueryable();
            return entities;
        }
        public T GetById(int id)
        {
            var entity = GetQueryable()
                .FirstOrDefault(x => x.Id == id);
            return entity;
        }
        public int? Create(T entity)
        {
            int? id = null;
            if (entity != null)
            {
                var entityType = typeof(T).Name;
                _log.Info(string.Format("Creating {0}", entityType));
                Add(entity);
                int unit = _UnitOfWork.SaveChanges();
                if (unit < 1 || entity.Id <= 0)
                {
                    _log.Error(string.Format("Failed to Create {0}", entityType, new InvalidOperationException()));
                }
                else
                {
                    _log.Info(string.Format("Created {0} with ID:{1}", entityType, entity.Id));
                    id = entity.Id;
                }
            }
            return id;
        }

        public int? UpdateEntity(T entity)
        {
            int? id = null;
            if (entity != null)
            {
                var entityType = typeof(T).Name;
                _log.Info(string.Format("Updating {0} with ID:{1}", entityType, entity.Id));
                Update(entity);
                int unit = _UnitOfWork.SaveChanges();
                if (unit < 1)
                {
                    _log.Error(string.Format("Failed to Update {0} with ID:{1}", entityType, entity.Id, new InvalidOperationException()));
                }
                else
                {
                    _log.Info(string.Format("Updated {0} with ID:{1}", entityType, entity.Id));
                    id = entity.Id;
                }
            }
            return id;
        }

        public bool DeleteEntity(T entity)
        {
            bool deleted = false;
            if (entity != null)
            {
                var entityType = typeof(T).Name;
                _log.Info(string.Format("Deleting {0} with ID:{1}", entityType, entity.Id));
                Delete(entity);
                int unit = _UnitOfWork.SaveChanges();
                if (unit < 1)
                {
                    _log.Error(string.Format("Failed to Delete {0} with ID:{1}", entityType, entity.Id, new InvalidOperationException()));
                }
                else
                {
                    _log.Info(string.Format("Deleted {0} with ID:{1}", entityType, entity.Id));
                    deleted = true;
                }
            }
            return deleted;
        }
    }
}
