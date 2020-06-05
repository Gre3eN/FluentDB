using FluentDB.Extensions.DbSpecific.Factories;
using FluentDB.Services;
using FluentDB.Services.Multiple;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace FluentDB.Extensions.DbSpecific
{
    public static class SqlQueryExtension
    {
        public static INewQueryable<SqlParameter> WithDefaultConnection(this Query<SqlCommand> query)
        {
            return SqlQueryableFactory.Create();
        }

        public static INewQueryable<SqlParameter> With(this Query<SqlCommand> query, string connectionString)
        {
            return SqlQueryableFactory.Create(connectionString);
        }

        public static IMultipleQueryable<TItem, SqlParameter> With<TItem>(
            this MultipleQuery<TItem, SqlCommand> query, string connectionString)
        {
            return SqlQueryableFactory.CreateMultiple(query.Collection, connectionString);
        }

        public static IMultipleQueryable<TItem, SqlParameter> WithDefaultConnection<TItem>(
            this MultipleQuery<TItem, SqlCommand> query)
        {
            return SqlQueryableFactory.CreateMultiple(query.Collection);
        }

        public static ISingleParameterQueryable<SqlParameter> Parameter(
            this ISingleParameterQueryable<SqlParameter> queryable, 
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

        public static IMultipleParameterQueryable<TItem, SqlParameter> Parameter<TItem>(
            this IMultipleParameterQueryable<TItem, SqlParameter> queryable,
            string id, SqlDbType type, Func<TItem, object> setValue,
            ParameterDirection direction = ParameterDirection.Input)
        {
            return queryable.Parameter(id, (param, item) =>
            {
                param.ParameterName = id;
                param.SqlDbType = type;
                param.Value = setValue(item);
                param.Direction = direction;
            });
        }
    }
}
