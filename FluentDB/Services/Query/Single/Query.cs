using FluentDB.Command;
using FluentDB.Factories;
using FluentDB.Services.Multiple;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace FluentDB.Services
{
    public class Query
    {
        private readonly Type commandType;

        private Query(Type commandType)
        {
            this.commandType = commandType;
        }

        /// <summary>
        /// Creates a new query with the currently active command type.
        /// </summary>
        /// <returns></returns>
        public static Query New()
        {
            return new Query(Configuration.ActiveCommandType 
                ?? throw new ArgumentNullException(nameof(Configuration.ActiveCommandType), "No Active Command set!")
                );
        }

        /// <summary>
        /// Creates a new query with <typeparamref name="TCommand"/> as command type.
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <returns></returns>
        public static Query New<TCommand>() where TCommand : DbCommand, new()
        {
            return new Query(typeof(TCommand));
        }

        /// <summary>
        /// Creates a Queryable with the default connection set in the configuration.
        /// </summary>
        /// <returns></returns>
        public INewQueryable WithDefaultConnection()
        {
            return QueryableFactory.New(
                ConfigurationBuilder.New(commandType)
                    .WithDefaultConnection()
                    .Build()
                );
        }

        /// <summary>
        /// Creates a Queryable with a specific connection.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public INewQueryable WithConnection(string connectionString)
        {
            return QueryableFactory.New(
                 ConfigurationBuilder.New(commandType)
                     .WithConnection(connectionString)
                     .Build()
                 );
        }

        /// <summary>
        /// Creates a Queryable with a transaction.
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public INewQueryable WithTransaction(DbTransaction transaction)
        {
            return QueryableFactory.New(
                 ConfigurationBuilder.New(commandType)
                     .WithTransaction(transaction)
                     .Build()
                 );
        }
    }
}
