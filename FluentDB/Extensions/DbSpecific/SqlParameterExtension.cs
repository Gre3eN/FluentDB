using FluentDB.Services;
using FluentDB.Services.Multiple;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace FluentDB.Extensions.DbSpecific
{
    public static class SqlParameterExtension
    {
        public static IParameterQueryable SqlType(this IParameterQueryable queryable,
            SqlDbType type)
        {
            return ((Queryable)queryable).ConfigureCurrentParameter<SqlParameter>(param => param.SqlDbType = type);
        }

        public static IParameterQueryable<TItem> SqlType<TItem>(this IParameterQueryable<TItem> queryable,
            SqlDbType type)
        {
            return ((MultipleQueryable<TItem>)queryable).ConfigureCurrentParameter<SqlParameter>(
                param => param.SqlDbType = type);
        }
    }
}
