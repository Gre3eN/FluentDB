using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace FluentDB.Factories
{
    public static class ConnectionFactory
    {
        private static readonly Dictionary<Type, IConnectionFactory> factoryTypeMap = new Dictionary<Type, IConnectionFactory>
        {
            { typeof(SqlCommand), new ConnectionFactory<SqlConnection>() }
        };

        public static IConnectionFactory New(Type commandType)
        {
            return factoryTypeMap[commandType];
        }
    }

    public class ConnectionFactory<TCon> : IConnectionFactory 
        where TCon : DbConnection, new()
    {
        public DbConnection Create(string connection)
        {
            return new TCon() { ConnectionString = connection };
        }
    }
}
