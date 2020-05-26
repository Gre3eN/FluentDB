using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection.Metadata;
using System.Text;

namespace FluentDB.Services
{
    public interface ISingleParameterQueryable<TParam> where TParam : DbParameter
    {
        ISingleParameterQueryable<TParam> Parameter(Action<TParam> configure);
        IResult Run();
    }
}
