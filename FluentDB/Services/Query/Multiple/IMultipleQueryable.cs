using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace FluentDB.Services.Multiple
{
    public interface IMultipleQueryable<TItem>
    {
        /// <summary>
        /// Configures the database query text.
        /// </summary>
        /// <param name="queryText"></param>
        /// <returns></returns>
        IMultipleConfigureQueryable<TItem> For(string queryText);
    }
}
