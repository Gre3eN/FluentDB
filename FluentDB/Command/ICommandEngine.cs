using System;
using System.Data.Common;

namespace FluentDB.Command
{
    public interface ICommandEngine
    {
        void AddConfiguration(Action<DbCommand> configuration);
        void Run(Action<DbCommand> commandAction);
    }
}