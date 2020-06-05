using FluentDB.Services;
using FluentDB.Services.Multiple;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace FluentDB.Extensions.DbSpecific.Factories
{
    internal static class SqlQueryableFactory
    {
        internal static INewQueryable<SqlParameter> Create(string connectionString = null)
        {
            return QueryableFactory.New<SqlCommand, SqlConnection, SqlTransaction, SqlParameter, SqlException>(
                connectionString);
        }

        internal static IMultipleQueryable<TItem, SqlParameter> CreateMultiple<TItem>(
            IEnumerable<TItem> collection,
            string connection = null)
        {
            return QueryableFactory.NewMultiple<TItem, SqlCommand, SqlConnection, SqlTransaction, SqlParameter, SqlException>(
                connection, collection);
        }
    }
}
