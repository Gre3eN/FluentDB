using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace FluentDB.Services
{
    public interface IConfigureQuerable<TParam> where TParam : DbParameter
    {
        IConfigureQuerable<TParam> CommandType(CommandType type);
        ISingleParameterQuerable<TParam> AsSingle();
    }
}
