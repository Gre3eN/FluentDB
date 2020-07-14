using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace FluentDB.Services
{
    public interface IParameterQueryable
    {
        IParameterQueryable Direction(ParameterDirection direction);
        IParameterQueryable ExplicitType(DbType type);
        IParameterQueryable Parameter(string paramId, object value);
        IResult Run();
    }
}
