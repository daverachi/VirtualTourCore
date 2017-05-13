using AutoMapper;
using VirtualTourCore.Common.Config;
using VirtualTourCore.Common.DataAccess.Interfaces;
using VirtualTourCore.Common.Exceptions;
using VirtualTourCore.Common.Extensions;
using VirtualTourCore.Common.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace VirtualTourCore.Common.DataAccess
{
    public abstract class ReadOnlyBaseRepository<T> : IReadOnlyBaseRepository<T>
        where T : class
    {
        protected IDbContext _Context { get; private set; }
        protected IList<Expression<Func<T, object>>> DefaultIncludes { get; private set; }
        protected DbSet<T> DbSet { get { return _Context.Set<T>(); } }
        public IUnitOfWork UnitOfWork { get; private set; }

        protected ReadOnlyBaseRepository(IUnitOfWork UoW)
        {
            _Context = UoW.GetRepositoryContext<T>();
            UnitOfWork = UoW;
            DefaultIncludes = new List<Expression<Func<T, object>>>();
        }

        public void AutoDetectChanges(bool enabled)
        {
            if (enabled)
            {
                _Context.Configuration.AutoDetectChangesEnabled = true;
            }
            else
            {
                _Context.Configuration.AutoDetectChangesEnabled = false;
            }
        }

        public virtual async Task<T> FindAsync(params object[] keyValues)
        {
            return await DbSet.FindAsync(keyValues);
        }

        public virtual T Find(params object[] keyValues)
        {
            return DbSet.Find(keyValues);
        }

        public virtual async Task<IList<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = AddDefaultIncludes(DbSet);
            return await QueryAsync(query.Where(predicate));
        }

        public virtual IList<T> Find(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = AddDefaultIncludes(DbSet);
            return Query(query.Where(predicate));
        }

        public virtual IQueryable<T> GetQueryable()
        {
            return AddDefaultIncludes(DbSet);
        }

        public virtual async Task<IList<U>> QueryAsync<U>(IQueryable<U> queryable)
        {
            return await ConvertQueryable(queryable).ToListAsync<U>();
        }

        public virtual async Task<int> CountAsync<U>(IQueryable<U> queryable)
        {
            return await ConvertQueryable(queryable).CountAsync<U>();
        }

        public virtual IList<U> Query<U>(IQueryable<U> queryable)
        {
            return ConvertQueryable(queryable).ToList<U>();
        }

        protected DbQuery<U> ConvertQueryable<U>(IQueryable<U> queryable)
        {
            DbQuery<U> dbquery = queryable as DbQuery<U>;
            if (dbquery == null)
            {
                throw new NotSupportedException("Queryable must be DbQuery.  Did you pass an IQueryable that was returned from GetQueryable()?");
            }
            return dbquery;
        }

        public void IncludeInQuery(IQueryable<T> query, Expression<Func<T, object>> expr)
        {
            if (!DefaultIncludes.Contains(expr))
            {
                query.Include(expr);
            }
        }

        private object GetEntityKey(T entity)
        {
            // NOTE: This will not work right if entity has multi-column key
            var objectStateEntry = _Context.ObjectContext().ObjectStateManager.GetObjectStateEntry(entity);
            return objectStateEntry.EntityKey.EntityKeyValues[0].Value;
        }

        private IQueryable<T> AddDefaultIncludes(IQueryable<T> query)
        {
            foreach (var include in DefaultIncludes)
                query = query.Include(include);
            return query;
        }

        public IEnumerable<T> Execute(string commandText, Dictionary<string, object> parameters, CommandType commandType)
        {
            return Execute<T>(commandText, parameters, commandType);
        }

        // TODO: Utilize IUnitOfWork, and create a DbTransaction manager for IUnitOfWork. This applies to all of the Execute.*(.*) methods below.
        // TODO: RE-FACTOR Executes to use a single private method for executing the actual query to keep from all of this duplicate connection code.
        public IEnumerable<T> Execute<T>(string commandText, Dictionary<string, object> parameters, CommandType commandType)
        {
            Asserter.AssertIsNotNullOrEmptyString("commandText", commandText);
            Asserter.AssertIsNotNull("parameters", parameters);
            List<T> results = new List<T>();
            DbContext dbContext = (DbContext)_Context;
            try
            {
                if (dbContext.Database.Connection.State != System.Data.ConnectionState.Open)
                {
                    dbContext.Database.Connection.Open();
                }

                using (DbCommand dbCommand = dbContext.Database.Connection.CreateCommand())
                {
                    dbCommand.CommandTimeout = CommonSettingsConfigSection.GetSection().DbContextCommandTimeout;
                    dbCommand.CommandText = commandText;
                    dbCommand.CommandType = commandType;

                    foreach (string key in parameters.Keys)
                    {
                        DbParameter dbParameter = dbCommand.CreateParameter();
                        dbParameter.ParameterName = key;
                        dbParameter.Value = parameters[key];
                        dbCommand.Parameters.Add(dbParameter);
                    }

                    using (DbDataReader dataReader = dbCommand.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            Mapper.CreateMap<IDataReader, T>().IgnoreAllNonExisting();
                            results = Mapper.Map<IDataReader, List<T>>(dataReader);
                        }
                    }
                }
            }
            finally
            {
                if (dbContext.Database.Connection != null && dbContext.Database.Connection.State != ConnectionState.Closed)
                {
                    dbContext.Database.Connection.Close();
                }
            }

            return results;
        }

        public SearchResults<T> ExecuteWithPaging(string commandText, Dictionary<string, object> parameters, CommandType commandType, PageSort pageSort)
        {
            Asserter.AssertIsNotNullOrEmptyString("commandText", commandText);
            Asserter.AssertIsNotNull("parameters", parameters);

            SearchResults<T> searchResults = new SearchResults<T>();

            DbContext dbContext = (DbContext)_Context;
            try
            {
                if (dbContext.Database.Connection.State != System.Data.ConnectionState.Open)
                {
                    dbContext.Database.Connection.Open();
                }

                using (DbCommand dbCommand = dbContext.Database.Connection.CreateCommand())
                {
                    dbCommand.CommandTimeout = CommonSettingsConfigSection.GetSection().DbContextCommandTimeout;
                    dbCommand.CommandText = commandText;
                    dbCommand.CommandType = commandType;

                    if (pageSort != null)
                        dbCommand.Parameters.AddPageSort(pageSort);

                    foreach (string key in parameters.Keys)
                    {
                        DbParameter dbParameter = dbCommand.CreateParameter();
                        dbParameter.ParameterName = key;
                        dbParameter.Value = parameters[key];
                        dbCommand.Parameters.Add(dbParameter);
                    }

                    using (DbDataReader dataReader = dbCommand.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            List<T> results = new List<T>();
                            Mapper.CreateMap<IDataReader, T>().IgnoreAllNonExisting();
                            results = Mapper.Map<IDataReader, List<T>>(dataReader);
                            if (results != null)
                            {
                                searchResults.Results = results;
                            }
                            else
                            {
                                searchResults.Results = new List<T>();
                            }
                            if (dataReader.NextResult())
                            {
                                if (dataReader.Read())
                                {
                                    searchResults.RowsAffected = Convert.ToInt32(dataReader[0]);
                                }
                                else
                                {
                                    throw new DataException("The query or stored procedure for a paged query must return TWO record sets, the record set of the actual records in the first record set and the number of total rows as a scalar value in the second record set.");
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                if (dbContext.Database.Connection != null && dbContext.Database.Connection.State != ConnectionState.Closed)
                {
                    dbContext.Database.Connection.Close();
                }
            }

            return searchResults;
        }

        public int ExecuteNonQuery(string commandText, Dictionary<string, object> parameters, CommandType commandType)
        {
            return ExecuteNonQuery(commandText, parameters, commandType, false);
        }

        /// <summary>
        /// Executes the specified command text as a non-query returning the number of records affected. If you need
        /// the records affected, be sure that SET NOCOUNT OFF is enabled so it can return the number of records
        /// affected, otherwise, it will return -1.
        /// </summary>
        /// <param name="dbContext">The IDbContext to use.</param>
        /// <param name="commandText">The command text to execute.</param>
        /// <param name="parameters">The parameters to use during execution.</param>
        /// <param name="commandType">The type of command the command text is.</param>
        /// <returns>Returns a System.Int32 based on the number of record affected during the specified query.</returns>       
        public int ExecuteNonQuery(string commandText, Dictionary<string, object> parameters, CommandType commandType, bool useReturnForCount)
        {
            Asserter.AssertIsNotNullOrEmptyString("commandText", commandText);
            Asserter.AssertIsNotNull("parameters", parameters);

            int recordsAffected = 0;
            DbContext dbContext = (DbContext)_Context;
            try
            {
                if (dbContext.Database.Connection.State != System.Data.ConnectionState.Open)
                {
                    dbContext.Database.Connection.Open();
                }

                using (DbCommand dbCommand = dbContext.Database.Connection.CreateCommand())
                {
                    dbCommand.CommandTimeout = CommonSettingsConfigSection.GetSection().DbContextCommandTimeout;
                    dbCommand.CommandText = commandText;
                    dbCommand.CommandType = commandType;

                    foreach (string key in parameters.Keys)
                    {
                        DbParameter dbParameter = dbCommand.CreateParameter();
                        dbParameter.ParameterName = key;
                        dbParameter.Value = parameters[key];
                        dbCommand.Parameters.Add(dbParameter);
                    }

                    // Uses the RETURN value of the stored procedure for the count, otherwise, 
                    // uses the total @@ROWCOUNT, will return -1 if SET NOCOUNT ON is specified. -cbb
                    if (useReturnForCount)
                    {
                        DbParameter dbParameter = dbCommand.CreateParameter();
                        dbParameter.Direction = ParameterDirection.ReturnValue;
                        dbParameter.ParameterName = "returnValue";
                        dbCommand.Parameters.Add(dbParameter);
                        dbCommand.ExecuteNonQuery();

                        if (dbParameter != null && dbParameter.Value != DBNull.Value)
                        {
                            recordsAffected = (int)dbParameter.Value;
                        }
                    }
                    else
                    {
                        recordsAffected = dbCommand.ExecuteNonQuery();
                    }
                }
            }
            finally
            {
                if (dbContext.Database.Connection != null && dbContext.Database.Connection.State != ConnectionState.Closed)
                {
                    dbContext.Database.Connection.Close();
                }
            }

            return recordsAffected;
        }

        public T ExecuteScalar<T>(string commandText, Dictionary<string, object> parameters, CommandType commandType)
        {
            return (T)ExecuteScalar(commandText, parameters, commandType);
        }

        public object ExecuteScalar(string commandText, Dictionary<string, object> parameters, CommandType commandType)
        {
            Asserter.AssertIsNotNullOrEmptyString("commandText", commandText);
            Asserter.AssertIsNotNull("parameters", parameters);

            object scalarValue = null;
            DbContext dbContext = (DbContext)_Context;
            try
            {
                if (dbContext.Database.Connection.State != System.Data.ConnectionState.Open)
                {
                    dbContext.Database.Connection.Open();
                }

                using (DbCommand dbCommand = dbContext.Database.Connection.CreateCommand())
                {
                    dbCommand.CommandTimeout = CommonSettingsConfigSection.GetSection().DbContextCommandTimeout;
                    dbCommand.CommandText = commandText;
                    dbCommand.CommandType = commandType;

                    foreach (string key in parameters.Keys)
                    {
                        DbParameter dbParameter = dbCommand.CreateParameter();
                        dbParameter.ParameterName = key;
                        dbParameter.Value = parameters[key];
                        dbCommand.Parameters.Add(dbParameter);
                    }
                    scalarValue = dbCommand.ExecuteScalar();
                }
            }
            finally
            {
                if (dbContext.Database.Connection != null && dbContext.Database.Connection.State != ConnectionState.Closed)
                {
                    dbContext.Database.Connection.Close();
                }
            }
            return scalarValue;
        }
    }
}
