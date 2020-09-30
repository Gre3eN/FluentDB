using FluentDB.Factories;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace FluentDB.Command
{
    internal class IteratingCommandEngine<TItem> : ICommandEngine
    {
        private readonly IteratingCommandConfig<TItem> commandConfig;
        private readonly List<Action<DbCommand>> configurations;
        private readonly List<Action<DbCommand, TItem>> paramConfiguration;

        internal IteratingCommandEngine(IteratingCommandConfig<TItem> commandConfig)
        {
            this.commandConfig = commandConfig ?? throw new ArgumentNullException(nameof(commandConfig));
            configurations = new List<Action<DbCommand>>();
            paramConfiguration = new List<Action<DbCommand, TItem>>();
        }

        public void AddConfiguration(Action<DbCommand> configuration)
        {
            configurations.Add(configuration);
        }

        public void AddParamConfiguration(Action<DbCommand, TItem> setParameter)
        {
            paramConfiguration.Add(setParameter);
        }

        public void Run(Action<DbCommand> commandAction)
        {
            try
            {
                using var command = CommandFactory.New(commandConfig.Static.CommandType);
                configurations.ForEach(c => c(command));
                if (SetTransactionIfPossible(command))
                {
                    ExecuteCommandAction(command, commandAction);
                }
                else
                {
                    using var connection = ConnectionFactory.New(
                        commandConfig.Static.CommandType, commandConfig.Static.ConnectionString);
                    connection.Open();
                    using var transaction = connection.BeginTransaction();
                    command.Transaction = transaction;
                    command.Connection = connection;
                    ExecuteCommandAction(command, commandAction);
                    transaction.Commit();
                }
            }
            catch (DbException ex)
            {
                throw ex;
                //TODO
            }
        }

        private bool SetTransactionIfPossible(DbCommand command)
        {
            if (commandConfig.Static.Transaction != null)
            {
                command.Transaction = commandConfig.Static.Transaction;
                command.Connection = commandConfig.Static.Transaction.Connection;
                return true;
            }
            return false;
        }

        private void ExecuteCommandAction(DbCommand command, Action<DbCommand> commandAction)
        {
            foreach (var item in commandConfig.Collection)
            {
                paramConfiguration.ForEach(c => c(command, item));
                commandAction(command);
            }
        }
    }
}
