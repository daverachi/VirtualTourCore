using VirtualTourCore.Common.DataAccess.Interfaces;
using VirtualTourCore.Common.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;

namespace VirtualTourCore.Common.DataAccess
{
    public abstract class UnitOfWorkBase
    {
        private Hashtable _repositories;
        protected List<IDbContext> _DbContexts = new List<IDbContext>();

        public IDbContext GetRepositoryContext<T>() where T : class
        {
            foreach (var _ctx in _DbContexts)
                if (_ctx.SetExists<T>())
                    return _ctx;
            return null; //uh-oh
        }

        public int SaveChanges()
        {
            using (TransactionScope _Transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions()
            {
                IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
                Timeout = CommonConfiguration.Configuration.TransactionScope_Timeout
            }))
            {
                int _retval = 0;
                foreach (var _ctx in _DbContexts)
                    _retval = _ctx.SaveChanges();
                _Transaction.Complete();
                return _retval;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            using (TransactionScope _Transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions()
            {
                IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
                Timeout = CommonConfiguration.Configuration.TransactionScope_Timeout
            }))
            {
                int _retval = 0;
                foreach (var _ctx in _DbContexts)
                    _retval = await _ctx.SaveChangesAsync();
                _Transaction.Complete();
                return _retval;
            }
        }

        public IRepository<T> Repository<T>() where T : class
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);
                var repositoryInstance =
                    Activator.CreateInstance(repositoryType
                            .MakeGenericType(typeof(T)), this);

                _repositories.Add(type, repositoryInstance);
            }

            return (IRepository<T>)_repositories[type];
        }
    }
}
