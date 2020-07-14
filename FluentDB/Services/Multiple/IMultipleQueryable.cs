using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace FluentDB.Services.Multiple
{
    public interface IMultipleQueryable<TItem>
    {
        IMultipleConfigureQueryable<TItem> For(string queryText);
    }
}
