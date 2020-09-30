using FluentDB.Factories;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace FluentDB.Services.Transaction
{
    public class TransactionService
    {
        private readonly Type commandType;

        private string optionalConnectionStr;

        private TransactionService(Type commandType)
        {
            this.commandType = commandType ?? throw new ArgumentNullException(nameof(commandType));
        }

        /// <summary>
        /// Creates a new TransactionService with the currently active command type.
        /// </summary>
        /// <returns></returns>
        public static TransactionService New()
        {
            return new TransactionService(Configuration.ActiveCommandType
                ?? throw new ArgumentNullException(nameof(Configuration.ActiveCommandType), "No Active Command set!")
                );
        }

        /// <summary>
        /// Creates a new TransactionService with the given <typeparamref name="TCommand"/>.
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <returns></returns>
        public static TransactionService New<TCommand>() where TCommand : DbCommand
        {
            return new TransactionService(typeof(TCommand));
        }

        /// <summary>
        /// Uses the given connection string instead of the default connection.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public TransactionService UsingConnection(string connectionString)
        {
            optionalConnectionStr = connectionString;
            return this;
        }

        /// <summary>
        /// Executes the given Action inside a transaction based on the command type this service 
        /// was initialized with.
        /// </summary>
        /// <param name="execute"></param>
        public void ExecuteInTransaction(Action<DbTransaction> execute)
        {
            var commandConfig = Configuration.GetCommandConfig(commandType);
            var connectionStr = optionalConnectionStr 
                ?? commandConfig.ConnectionString 
                ?? throw new ArgumentNullException(nameof(commandConfig.ConnectionString), "Default Connection not set!");
            using var connection = ConnectionFactory.New(commandType, connectionStr);
            connection.Open();
            using var transaction = connection.BeginTransaction();
            execute(transaction);
            transaction.Commit();
        }
    }
}
