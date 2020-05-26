using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace FluentDB.Services.Multiple
{
    public interface IMultipleQueryable<TItem, TParam> where TParam : DbParameter
    {
        IMultipleConfigureQueryable<TItem, TParam> For(string queryText);
    }
}
