using System;
using System.Data.Common;

namespace FluentDB.Command
{
    public interface ICommandEngine<TCommand> where TCommand : DbCommand, new()
    {
        void AddConfiguration(CommandConfiguration<TCommand> configuration);
        void Run(Action<TCommand> commandAction);
        T Run<T>(Func<TCommand, T> commandFunction);
    }
}