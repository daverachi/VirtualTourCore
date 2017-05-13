using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace VirtualTourCore.Common.DataAccess.Interfaces
{
    public interface IDbContext : IDisposable
    {
        DbContextConfiguration Configuration { get; }
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        ObjectContext ObjectContext();
        Task<int> SaveChangesAsync();
        int SaveChanges();
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        bool SetExists<T>() where T : class;
    }
}
