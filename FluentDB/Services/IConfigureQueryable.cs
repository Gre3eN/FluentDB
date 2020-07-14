using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace FluentDB.Services
{
    public interface IConfigureQueryable
    {
        IConfigureQueryable CommandType(CommandType type);
        IFirstParameterQueryable WithParameters();
        IResult Run();
    }
}
