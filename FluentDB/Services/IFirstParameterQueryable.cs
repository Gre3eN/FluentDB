using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection.Metadata;
using System.Text;

namespace FluentDB.Services
{
    public interface IFirstParameterQueryable
    {
        IParameterQueryable Parameter(string paramId, object value);
        IResult Run();
    }
}
