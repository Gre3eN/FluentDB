using FluentDB.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace FluentDB.Extensions.DbSpecific
{
    public static class IEnumerableExtension
    {
        public static IMultipleQueryable<SqlParameter> AsDatabaseQuery<T>(this IEnumerable<T> collection)
        {
            return Query<SqlCommand>.New().WithDefaultConnection();
        }
    }
}
