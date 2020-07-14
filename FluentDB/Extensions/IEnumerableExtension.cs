using FluentDB.Services;
using FluentDB.Services.Multiple;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace FluentDB.Extensions
{
    public static class IEnumerableExtension
    {
        public static MultipleQuery<T> AsDatabaseQuery<T>(this IEnumerable<T> collection)
        {
            return new MultipleQuery<T>(collection);
        }
    }
}
