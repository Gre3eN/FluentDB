using System;
using System.Data.Common;

namespace FluentDB.Command
{
    public interface ICommandEngine<TCommand> where TCommand : DbCommand, new()
    {
        void AddConfiguration(Action<TCommand> configuration);
        void Run(Action<TCommand> commandAction);
    }
}