using FluentDB.Factories;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace FluentDB.Command
{
    internal class CommandEngine : ICommandEngine, IReturnCommandEngine
    {
        private readonly StaticCommandConfig commandConfig;
        private readonly List<Action<DbCommand>> configurations;

        internal CommandEngine(StaticCommandConfig commandConfig)
        {
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
                using var command = CommandFactory.New(commandConfig.CommandType);
                configurations.ForEach(c => c(command));
                if (SetTransactionIfPossible(command))
                {
                    commandAction(command);
                }
                else
                {
                    using var connection = ConnectionFactory.New(commandConfig.CommandType, commandConfig.ConnectionString);
                    connection.Open();
                    command.Connection = connection;
                    commandAction(command);
                }
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
                using var command = CommandFactory.New(commandConfig.CommandType);
                configurations.ForEach(c => c(command));
                if (SetTransactionIfPossible(command))
                {
                    return commandFunction(command);
                }
                else
                {
                    using var connection = ConnectionFactory.New(commandConfig.CommandType, commandConfig.ConnectionString);
                    connection.Open();
                    command.Connection = connection;
                    return commandFunction(command);
                }
            }
            catch (DbException ex)
            {
                throw ex;
                //TODO
                //throw new InvalidOperationException();
            }
        }

        private bool SetTransactionIfPossible(DbCommand command)
        {
            if (commandConfig.Transaction != null)
            {
                command.Transaction = commandConfig.Transaction;
                command.Connection = commandConfig.Transaction.Connection;
                return true;
            }
            return false;
        }
    }
}
