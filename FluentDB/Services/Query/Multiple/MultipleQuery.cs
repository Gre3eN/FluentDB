using FluentDB.Command;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace FluentDB.Services.Multiple
{
    public class MultipleQuery<TItem>
    {
        private readonly IEnumerable<TItem> collection;

        private Type commandType;

        internal MultipleQuery(IEnumerable<TItem> collection)
        {
            this.collection = collection ?? throw new ArgumentNullException(nameof(collection));
            commandType = Configuration.ActiveCommandType 
                ?? throw new ArgumentNullException(nameof(Configuration.ActiveCommandType), "No Active Command set!");
        }

        /// <summary>
        /// Uses <typeparamref name="TCommand"/> with the query instead of the currently active command type.
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <returns></returns>
        public MultipleQuery<TItem> Using<TCommand>() where TCommand : DbCommand, new()
        {
            commandType = typeof(TCommand);
            return this;
        }

        /// <summary>
        /// Creates a Queryable with the default connection set in the configuration.
        /// </summary>
        /// <returns></returns>
        public IMultipleQueryable<TItem> WithDefaultConnection()
        {
            return QueryableFactory.NewMultiple(
                ConfigurationBuilder.New(commandType)
                    .WithDefaultConnection()
                    .BuildAsMultiple(collection)
                );
        }

        /// <summary>
        /// Creates a Queryable with a specific connection.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public IMultipleQueryable<TItem> WithConnection(string connectionString)
        {
            return QueryableFactory.NewMultiple(
                ConfigurationBuilder.New(commandType)
                    .WithConnection(connectionString)
                    .BuildAsMultiple(collection)
                );
        }

        /// <summary>
        /// Creates a Queryable with a transaction.
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public IMultipleQueryable<TItem> WithTransaction(DbTransaction transaction)
        {
            return QueryableFactory.NewMultiple(
                ConfigurationBuilder.New(commandType)
                    .WithTransaction(transaction)
                    .BuildAsMultiple(collection)
                );
        }
    }
}
