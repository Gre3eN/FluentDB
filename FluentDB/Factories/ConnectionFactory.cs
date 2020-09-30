using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Text;

namespace FluentDB.Factories
{
    public static class ConnectionFactory
    {
        private static readonly Dictionary<Type, Func<string, DbConnection>> connectionTypeMap = new Dictionary<Type, Func<string, DbConnection>>
        {
            { typeof(SqlCommand), conStr => new SqlConnection(conStr) },
            { typeof(OleDbCommand), conStr => new OleDbConnection(conStr) }
        };

        public static DbConnection New(Type commandType, string connectionStr)
        {
            return connectionTypeMap[commandType](connectionStr);
        }
    }
}
