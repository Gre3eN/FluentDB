using FluentDB.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace FluentDB.Extensions.DbSpecific
{
    public static class SqlQueryExtension
    {
        public static INewQuerable<SqlParameter> WithDefaultConnection(this Query<SqlCommand> query)
        {
            return query.WithDefaultConnection<SqlConnection, SqlTransaction, SqlParameter, SqlException>();
        }

        public static ISingleParameterQuerable<SqlParameter> Parameter(
            this ISingleParameterQuerable<SqlParameter> queryable, 
            string id, SqlDbType type, object value,
            ParameterDirection direction = ParameterDirection.Input)
        {
            return queryable.Parameter(param =>
            {
                param.ParameterName = id;
                param.SqlDbType = type;
                param.Value = value;
                param.Direction = direction;
            });
        }
    }
}
