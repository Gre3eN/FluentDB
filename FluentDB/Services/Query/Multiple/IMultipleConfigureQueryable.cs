using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace FluentDB.Services.Multiple
{
    public interface IMultipleConfigureQueryable<TItem>
    {
        /// <summary>
        /// Configures the CommandType.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IMultipleConfigureQueryable<TItem> CommandType(CommandType type);
        /// <summary>
        /// Allows to configure parameters for the query.
        /// </summary>
        /// <returns></returns>
        IFirstParameterQueryable<TItem> WithParameters();
        /// <summary>
        /// Allows to run the query.
        /// </summary>
        /// <returns></returns>
        IRunMultiple Run();
    }
}
