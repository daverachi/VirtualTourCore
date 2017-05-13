using VirtualTourCore.Common.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace VirtualTourCore.Common.DataAccess.Interfaces
{
    public interface IReadOnlyBaseRepository<T>
    where T : class
    {
        Task<T> FindAsync(params object[] keyValues);
        T Find(params object[] keyValues);
        Task<IList<T>> FindAsync(Expression<Func<T, bool>> predicate);
        IList<T> Find(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetQueryable();
        Task<IList<U>> QueryAsync<U>(IQueryable<U> queryable);
        Task<int> CountAsync<U>(IQueryable<U> queryable);
        IList<U> Query<U>(IQueryable<U> queryable);
        IUnitOfWork UnitOfWork { get; }
        void IncludeInQuery(IQueryable<T> query, Expression<Func<T, object>> expr);
        IEnumerable<T> Execute(string commandText, Dictionary<string, object> parameters, CommandType commandType);
        IEnumerable<T> Execute<T>(string commandText, Dictionary<string, object> parameters, CommandType commandType);
        SearchResults<T> ExecuteWithPaging(string commandText, Dictionary<string, object> parameters, CommandType commandType, PageSort pageSort);
        int ExecuteNonQuery(string commandText, Dictionary<string, object> parameters, CommandType commandType);
        int ExecuteNonQuery(string commandText, Dictionary<string, object> parameters, CommandType commandType, bool useReturnForCount);
        object ExecuteScalar(string commandText, Dictionary<string, object> parameters, CommandType commandType);
        T ExecuteScalar<T>(string commandText, Dictionary<string, object> parameters, CommandType commandType);
    }
}
