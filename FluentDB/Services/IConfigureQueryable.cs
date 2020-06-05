using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace FluentDB.Services
{
    public interface IConfigureQueryable<TParam> where TParam : DbParameter
    {
        IConfigureQueryable<TParam> CommandType(CommandType type);
        ISingleParameterQueryable<TParam> WithParameters();
        IResult Run();
    }
}
