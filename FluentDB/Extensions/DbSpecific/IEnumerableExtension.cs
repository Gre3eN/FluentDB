using FluentDB.Services;
using FluentDB.Services.Multiple;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace FluentDB.Extensions.DbSpecific
{
    public static class IEnumerableExtension
    {
        public static IMultipleQueryable<T, SqlParameter> AsDatabaseQuery<TCommand, T>(this IEnumerable<T> collection)
             where TCommand : DbCommand
        {
            return NewMultiple<TCommand, T>(collection);
        }

        public static IMultipleQueryable<T, SqlParameter> AsDatabaseQuery<TCommand, T>(this IEnumerable<T> collection,
            string connectionString) where TCommand : DbCommand
        {
            return NewMultiple<TCommand, T>(collection, connectionString);
        }

        internal static IMultipleQueryable<TItem, SqlParameter> NewMultiple<TCommand, TItem>(
            IEnumerable<TItem> collection,
            string connection = null) where TCommand : DbCommand
        {
            return (typeof(TCommand)) switch
            {
                Type type when type == typeof(SqlCommand) 
                    => Query<SqlCommand>.New()
                        .NewMultiple<TItem, SqlConnection, SqlTransaction, SqlParameter, SqlException>(
                            collection, connection),
                _ => throw new ArgumentException($"Type {typeof(TCommand)} not defined"),
            };
        }
    }
}
