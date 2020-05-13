using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace FluentDB.Services
{
    public class Query<TCommand> where TCommand : DbCommand, new()
    {
        private static string connectionString;

        public static void ConfigureConnection(string connectionStr)
        {
            connectionString = connectionStr;
        }

        public static Query<TCommand> New()
        {
            return new Query<TCommand>(connectionString);
        }

        private readonly string? connectionStr;

        private Query(string? connectionStr)
        {
            this.connectionStr = connectionStr;
        }

        internal INewQuerable<TParam> WithConnection<TCon, TTrans, TParam>() 
            where TCon : DbConnection, new()
            where TTrans : DbTransaction
            where TParam : DbParameter
        {
            var con = new TCon
            {
                ConnectionString = connectionStr
                ?? throw new ArgumentException("No default connection string defined.")
            };
            var command = new TCommand
            {
                Connection = con
            };
            return new Querable<TCommand, TCon, TTrans, TParam>(command);
        }
    }
}
