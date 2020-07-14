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

        private Type optionalCommandType;

        internal MultipleQuery(IEnumerable<TItem> collection)
        {
            this.collection = collection ?? throw new ArgumentNullException(nameof(collection));
        }

        public MultipleQuery<TItem> Using<TCommand>() where TCommand : DbCommand, new()
        {
            optionalCommandType = typeof(TCommand);
            return this;
        }

        public IMultipleQueryable<TItem> With(string connectionString)
        {
            return QueryableFactory.NewMultiple(collection, optionalCommandType, connectionString);
        }

        public IMultipleQueryable<TItem> WithDefaultConnection()
        {
            return QueryableFactory.NewMultiple(collection, optionalCommandType);
        }
    }
}
