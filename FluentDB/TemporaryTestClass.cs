using FluentDB.DbSpecificExtensions;
using FluentDB.Services;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace FluentDB
{
    public class TemporaryTestClass
    {
        public void Test()
        {
            Query<SqlCommand>.New()
                .WithConnection()
                .For("query")
                .CommandType(System.Data.CommandType.StoredProcedure)
                .AsSingle()
                .Parameter(param =>
                {
                    param.ParameterName = "@Id";
                    param.SqlDbType = System.Data.SqlDbType.Int;
                    param.Value = 1;
                })
                .Run()
                .AsEnumerable(reader =>
                {
                    return reader.GetString(reader.GetOrdinal("bla"));
                })
                .Any();
        }
    }
}
