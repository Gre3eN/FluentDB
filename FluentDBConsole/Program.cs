using FluentDB.Extensions.DbSpecific;
using FluentDB.Services;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace FluentDBConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = Configuration.New<SqlCommand>();
            config.ConfigureDefaultConnection("Server = .,51997; Database = Profi32; User Id = sa; Password = express;");

            Test();
        }

        static void Test()
        {
            var result = Query.New()
                .WithDefaultConnection()
                .For("SELECT Name1 FROM Adressen WHERE TypKennung = @TypKennung")
                .WithParameters()
                .Parameter("@TypKennung", "K")
                .Run()
                .AsScalar(result => (string)result);
                //.As(reader => reader.GetSchemaTable())
                //.AsNonQuery()
                //.AsEnumerable(reader =>
                //{
                //    return reader.GetString(reader.GetOrdinal("bla"));
                //}).ToList();
        }
    }
}
