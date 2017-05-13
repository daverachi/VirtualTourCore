using VirtualTourCore.Common.Helper;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace VirtualTourCore.Common.DataAccess
{
    public static class DataHelper
    {
        public static Nullable<int> GetInt32(this IDataRecord reader, string columnName)
        {
            var index = reader.GetOrdinal(columnName);
            return reader.IsDBNull(index) ? null : (Nullable<int>)reader.GetInt32(index);
        }
        public static string GetString(this IDataRecord reader, string columnName)
        {
            var index = reader.GetOrdinal(columnName);
            return reader.IsDBNull(index) ? null : (string)reader.GetString(index);
        }
        public static void Add<T>(this DbParameterCollection parameters, string name, T item)
        {
            if (item != null)
            {
                parameters.Add(new SqlParameter(name, item));
            }
        }
        public static void AddPageSort(this DbParameterCollection parameters, PageSort item)
        {
            if (item != null)
            {
                parameters.Add(new SqlParameter("PageNumber", item.PageNumber));
                parameters.Add(new SqlParameter("PageSize", item.PageSize));

                if (item.OrderBy != null && item.OrderBy.Count > 0)
                {
                    var orderBy = item.OrderBy.First();
                    parameters.Add(new SqlParameter("Column", orderBy.Column));
                    parameters.Add(new SqlParameter("SortDirection", orderBy.Direction));
                }
            }
        }
        public static void AddParameter<T>(this DbCommand command, string name, T item)
        {
            if (command != null)
            {
                command.Parameters.Add(new SqlParameter(name, item));
            }
        }
        public static void AddParameter(this DbCommand command, PageSort item)
        {
            if (command != null)
            {
                command.Parameters.AddPageSort(item);
            }
        }
    }
}
