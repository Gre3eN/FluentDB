using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace FluentDB.Services
{
    public interface IResult
    {
        /// <summary>
        /// Runs the query and reads all lines of the result.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="read"></param>
        /// <returns></returns>
        IEnumerable<T> AsEnumerable<T>(Func<DbDataReader, T> read);
        /// <summary>
        /// Runs the query und reads the first line of the result.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="read"></param>
        /// <returns></returns>
        T As<T>(Func<DbDataReader, T> read);
        /// <summary>
        /// Runs the query and returns the first cell of the result.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="convert"></param>
        /// <returns></returns>
        T AsScalar<T>(Func<object, T> convert);
        /// <summary>
        /// Runs the query.
        /// </summary>
        void AsNonQuery();
        /// <summary>
        /// All parameters used in the query can be accessed threw their Id.
        /// </summary>
        Dictionary<string, DbParameter> Parameters { get; }
    }
}
