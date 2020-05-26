using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace FluentDB.Command
{
    public interface IReturnCommandEngine<TCommand> where TCommand : DbCommand, new()
    {
        T Run<T>(Func<TCommand, T> commandFunction);
    }
}
