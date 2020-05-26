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
        public void Test()
        {
            Query<SqlCommand>.New()
                .WithDefaultConnection()
                .For("SELECT * FROM Table WHERE Id = @Id AND Something = @Something")
                //.CommandType(CommandType.StoredProcedure)
                .With()
                .Parameter("@Id", SqlDbType.Int, 1)
                .Parameter("@Something", SqlDbType.VarChar, "bla")
                .Run()
                .AsEnumerable(reader =>
                {
                    return reader.GetString(reader.GetOrdinal("bla"));
                });
        }

        public void TestCollection(IEnumerable<string> collection)
        {
            collection.AsDatabaseQuery<SqlCommand, string>()
                .For("")
                .With()
                .Parameter("id", SqlDbType.Int, item => item)
                .Run()
                .AsNonQuery();
        }
    }
}
