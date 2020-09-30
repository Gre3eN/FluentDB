using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace FluentDB.Services
{
    public interface INewQueryable
    {
        /// <summary>
        /// Configures the database query text.
        /// </summary>
        /// <param name="queryText"></param>
        /// <returns></returns>
        IConfigureQueryable For(string queryText);
    }
}
