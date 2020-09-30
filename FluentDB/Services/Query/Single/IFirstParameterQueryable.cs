using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection.Metadata;
using System.Text;

namespace FluentDB.Services
{
    public interface IFirstParameterQueryable
    {
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
