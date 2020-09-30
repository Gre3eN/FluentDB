using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace FluentDB.Services
{
    public interface IParameterQueryable
    {
        /// <summary>
        /// Sets the direction of the parameter. Default is Input.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        IParameterQueryable Direction(ParameterDirection direction);
        /// <summary>
        /// Sets the type of the parameter explicitly.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IParameterQueryable ExplicitType(DbType type);
        /// <summary>
        /// Configures a parameter with the given id and value.
        /// </summary>
        /// <param name="paramId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IParameterQueryable Parameter(string paramId, object value);
        /// <summary>
        /// Allows to run the query.
        /// </summary>
        /// <returns></returns>
        IResult Run();
    }
}
