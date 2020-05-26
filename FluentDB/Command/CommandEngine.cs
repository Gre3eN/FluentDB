using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

namespace FluentDB.Command
{
    public class CommandEngine<TCommand, TCon, TDbEx> : ICommandEngine<TCommand>, IReturnCommandEngine<TCommand>
        where TCommand : DbCommand, new()
        where TCon : DbConnection, new()
        where TDbEx : DbException
    {
        private readonly StaticCommandConfig commandConfig;
        private readonly List<Action<TCommand>> configurations;

        public CommandEngine(StaticCommandConfig commandConfig)
        {
            this.commandConfig = commandConfig ?? throw new ArgumentNullException(nameof(commandConfig));
            this.configurations = new List<Action<TCommand>>();
        }

        public void AddConfiguration(Action<TCommand> configuration)
        {
            configurations.Add(configuration);
        }

        public void Run(Action<TCommand> commandAction)
        {
            try
            {
                using var command = new TCommand();
                configurations.ForEach(c => c(command));
                using var connection = command.Connection
                    ?? new TCon { ConnectionString = commandConfig.ConnectionString };
                connection.Open();
                commandAction(command);
            }
            catch (TDbEx ex)
            {
                //TODO
            }
        }

        public T Run<T>(Func<TCommand, T> commandFunction)
        {
            try
            {
                using var command = new TCommand();
                configurations.ForEach(c => c(command));
                using var connection = command.Connection
                    ?? new TCon { ConnectionString = commandConfig.ConnectionString };
                connection.Open();
                return commandFunction(command);
            }
            catch (TDbEx ex)
            {
                throw new InvalidOperationException();
            }
        }
    }
}
