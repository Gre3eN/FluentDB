using FluentDB.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace FluentDB.DbSpecificExtensions
{
    public static class SqlQueryExtension
    {
        public static INewQuerable<SqlParameter> WithConnection(this Query<SqlCommand> query)
        {
            return query.WithConnection<SqlConnection, SqlTransaction, SqlParameter>();
        }
    }
}
