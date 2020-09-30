using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace FluentDB.Services
{
    public interface IConfigureQueryable
    {
        /// <summary>
        /// Configures the CommandType.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IConfigureQueryable CommandType(CommandType type);
        /// <summary>
        /// Allows to configure parameters for the query.
        /// </summary>
        /// <returns></returns>
        IFirstParameterQueryable WithParameters();
        /// <summary>
        /// Allows to run the query.
        /// </summary>
        /// <returns></returns>
        IResult Run();
    }
}
