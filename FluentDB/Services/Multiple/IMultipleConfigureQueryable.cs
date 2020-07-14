using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace FluentDB.Services.Multiple
{
    public interface IMultipleConfigureQueryable<TItem>
    {
        IMultipleConfigureQueryable<TItem> CommandType(CommandType type);
        IParameterQueryable<TItem> WithParameters();
        IRunMultiple Run();
    }
}
