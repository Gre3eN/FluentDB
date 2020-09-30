using FluentDB.Services;
using FluentDB.Services.Multiple;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Text;

namespace FluentDB.Extensions.DbSpecific
{
    public static class AccParameterExtension
    {
        public static IParameterQueryable AccType(this IParameterQueryable queryable,
            OleDbType type)
        {
            return ((Queryable)queryable).ConfigureCurrentParameter<OleDbParameter>(param => param.OleDbType = type);
        }

        public static IParameterQueryable<TItem> AccType<TItem>(this IParameterQueryable<TItem> queryable,
            OleDbType type)
        {
            return ((MultipleQueryable<TItem>)queryable).ConfigureCurrentParameter<OleDbParameter>(
                param => param.OleDbType = type);
        }
    }
}
