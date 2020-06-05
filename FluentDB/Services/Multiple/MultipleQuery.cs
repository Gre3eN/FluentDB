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

        internal MultipleQuery(IEnumerable<TItem> collection)
        {
            this.collection = collection ?? throw new ArgumentNullException(nameof(collection));
        }

        public MultipleQuery<TItem, TCommand> Using<TCommand>() where TCommand : DbCommand, new()
        {
            return new MultipleQuery<TItem, TCommand>(collection);
        }
    }

    public class MultipleQuery<TItem, TCommand>
        where TCommand : DbCommand, new()
    {
        internal IEnumerable<TItem> Collection { get; }

        internal MultipleQuery(IEnumerable<TItem> collection)
        {
            Collection = collection ?? throw new ArgumentNullException(nameof(collection));
        }
    }
}
