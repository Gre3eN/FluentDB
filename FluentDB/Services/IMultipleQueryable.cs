using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace FluentDB.Services
{
    public interface IMultipleQueryable<TParam> where TParam : DbParameter
    {
        IMultipleConfigureQueryable<TParam> For(string queryText);
    }
}
