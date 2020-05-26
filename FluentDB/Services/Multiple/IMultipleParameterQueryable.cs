using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace FluentDB.Services.Multiple
{
    public interface IMultipleParameterQueryable<TItem, TParam> where TParam : DbParameter
    {
        IMultipleParameterQueryable<TItem, TParam> Parameter(string paramId, Action<TParam, TItem> configure);
        IRunMultiple Run();
    }
}
