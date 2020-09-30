using System;
using System.Collections.Generic;
using System.Text;

namespace FluentDB.Services.Multiple
{
    public interface IRunMultiple
    {
        /// <summary>
        /// Runs the query.
        /// </summary>
        void AsNonQuery();
    }
}
