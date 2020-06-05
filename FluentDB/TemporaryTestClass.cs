using FluentDB.Services;
using FluentDB.Extensions.DbSpecific;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Collections.Generic;

namespace FluentDB
{
    public class TemporaryTestClass
    {
        public string TestProperty { get; }

        public void Test()
        {
            Query<SqlCommand>.New()
                .WithDefaultConnection()
                .For("SELECT * FROM Table WHERE Id = @Id AND Something = @Something")
                //.CommandType(CommandType.StoredProcedure)
                .WithParameters()
                .Parameter("@Id", SqlDbType.Int, 1)
                .Parameter("@Something", SqlDbType.VarChar, "bla")
                .Run()
                .AsEnumerable(reader =>
                {
                    return reader.GetString(reader.GetOrdinal("bla"));
                });
        }

        public void TestCollection(IEnumerable<TemporaryTestClass> collection)
        {
            collection.AsDatabaseQuery()
                .Using<SqlCommand>()
                .WithDefaultConnection()
                .For("")
                .WithParameters()
                .Parameter("id", SqlDbType.Int, item => item.TestProperty)
                .Run()
                .AsNonQuery();
        }
    }
}
