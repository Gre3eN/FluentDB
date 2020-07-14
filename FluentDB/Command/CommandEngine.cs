using FluentDB.Factories;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;

namespace FluentDB.Command
{
    public class CommandEngine : ICommandEngine, IReturnCommandEngine
    {
        private readonly ICommandFactory commandFactory;
        private readonly IConnectionFactory connectionFactory;
        private readonly StaticCommandConfig commandConfig;
        private readonly List<Action<DbCommand>> configurations;

        public CommandEngine(ICommandFactory commandFactory, 
            IConnectionFactory connectionFactory, 
            StaticCommandConfig commandConfig)
        {
            this.commandFactory = commandFactory ?? throw new ArgumentNullException(nameof(commandFactory));
            this.connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            this.commandConfig = commandConfig ?? throw new ArgumentNullException(nameof(commandConfig));
            configurations = new List<Action<DbCommand>>();
        }

        public void AddConfiguration(Action<DbCommand> configuration)
        {
            configurations.Add(configuration);
        }

        public void Run(Action<DbCommand> commandAction)
        {
            try
            {
                using var command = commandFactory.Create();
                configurations.ForEach(c => c(command));
                using var connection = command.Connection
                    ?? connectionFactory.Create(commandConfig.ConnectionString);
                connection.Open();
                commandAction(command);
            }
            catch (DbException ex)
            {
                throw ex;
                //TODO
            }
        }

        public T Run<T>(Func<DbCommand, T> commandFunction)
        {
            try
            {
                using var command = commandFactory.Create();
                configurations.ForEach(c => c(command));
                using var connection = command.Connection
                    ?? connectionFactory.Create(commandConfig.ConnectionString);
                connection.Open();
                return commandFunction(command);
            }
            catch (DbException ex)
            {
                throw ex;
                //TODO
                //throw new InvalidOperationException();
            }
        }
    }
}
