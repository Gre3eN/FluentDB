using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace FluentDB.Services
{
    public interface IResult
    {
        IEnumerable<T> AsEnumerable<T>(Func<DbDataReader, T> read);
        T As<T>(Func<DbDataReader, T> read);
        T AsScalar<T>(Func<object, T> convert);
        void AsNonQuery();
        Dictionary<string, DbParameter> Parameters { get; }
    }
}
