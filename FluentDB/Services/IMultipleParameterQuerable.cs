using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace FluentDB.Services
{
    public interface IMultipleParameterQuerable<TParam> where TParam : DbParameter
    {
        IMultipleParameterQuerable<TParam> Parameter(Action<TParam, object> configure);
        IRunMultiple Run();
    }
}
