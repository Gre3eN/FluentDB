using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Text;

namespace FluentDB.Factories
{
    public static class CommandFactory
    {
        private static readonly Dictionary<Type, Func<DbCommand>> commandTypeMap = new Dictionary<Type, Func<DbCommand>>
        {
            { typeof(SqlCommand), () => new SqlCommand() },
            { typeof(OleDbCommand), () => new OleDbCommand() }
        };

        public static DbCommand New(Type commandType)
        {
            return commandTypeMap[commandType]();
        }
    }
}
