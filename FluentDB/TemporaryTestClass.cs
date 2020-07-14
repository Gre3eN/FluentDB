using FluentDB.Services;
using FluentDB.Extensions.DbSpecific;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Collections.Generic;
using FluentDB.Extensions;
using System.Data.OleDb;
using MySql.Data.MySqlClient;

namespace FluentDB
{
    public class TemporaryTestClass
    {
        public string TestProperty { get; }

        public void Setup()
        {
            //this would set the active command type (which is used in queries you don't explicitely set
            //a command type to be used with the query)
            var config = Configuration.New<MySqlCommand>();
            config.ConfigureConnection("connection string");

            //if you set multiple configurations (for different command types) the first will be set as active
            //command type
            var accConfig = Configuration.New<OleDbCommand>();
            accConfig.ConfigureConnection("connection string");

            //in this case the currently active command type would still be 'SqlCommand'
            //unless you set the active command type manually
            Configuration.SetActiveCommandTypeTo<OleDbCommand>();
        }

        public void Test()
        {
            //Query.New<SqlCommand>()
            Query.New()
                .WithDefaultConnection()
                .For("SELECT * FROM Table WHERE Id = @Id AND Something = @Something")
                //.CommandType(CommandType.StoredProcedure)
                .WithParameters()
                .Parameter("@Id", 1).SqlType(SqlDbType.Int)
                .Parameter("@Something", "bla")
                .Run()
                //.AsScalar(result => (int)result)
                //.As(reader => reader.GetSchemaTable())
                .AsEnumerable(reader =>
                {
                    return reader.GetString(reader.GetOrdinal("bla"));
                });
        }

        public void TestCollection(IEnumerable<TemporaryTestClass> collection)
        {
            collection.AsDatabaseQuery()
                //.Using<SqlCommand>()
                .WithDefaultConnection()
                .For("")
                .WithParameters()
                .Parameter("id", item => item.TestProperty)
                .Parameter("lala", item => item.TestProperty).Direction(ParameterDirection.Output).ExplicitType(DbType.String)
                .Run()
                .AsNonQuery();
        }
    }
}
