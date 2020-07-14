using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace FluentDB.Command
{
    public interface IReturnCommandEngine
    {
        T Run<T>(Func<DbCommand, T> commandFunction);
    }
}
