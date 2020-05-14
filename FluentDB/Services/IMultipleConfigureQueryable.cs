using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace FluentDB.Services
{
    public interface IMultipleConfigureQueryable<TParam> where TParam : DbParameter
    {
        IMultipleConfigureQueryable<TParam> CommandType(CommandType type);
        IMultipleParameterQuerable<TParam> With();
        IRunMultiple Run();
    }
}
