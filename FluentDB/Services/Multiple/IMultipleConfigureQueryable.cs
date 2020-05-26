using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace FluentDB.Services.Multiple
{
    public interface IMultipleConfigureQueryable<TItem, TParam> where TParam : DbParameter
    {
        IMultipleConfigureQueryable<TItem, TParam> CommandType(CommandType type);
        IMultipleParameterQueryable<TItem, TParam> With();
        IRunMultiple Run();
    }
}
