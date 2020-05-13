using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection.Metadata;
using System.Text;

namespace FluentDB.Services
{
    public interface ISingleParameterQuerable<TParam> where TParam : DbParameter
    {
        ISingleParameterQuerable<TParam> Parameter(Action<TParam> configure);
        IResult Run();
    }
}
