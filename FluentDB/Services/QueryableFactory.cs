using FluentDB.Command;
using FluentDB.Services.Multiple;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace FluentDB.Services
{
    public static class QueryableFactory
    {
        internal static INewQueryable<TParam> New<TCommand, TCon, TTrans, TParam, TDbEx>(string connection)
            where TCommand : DbCommand, new()
            where TCon : DbConnection, new()
            where TTrans : DbTransaction
            where TParam : DbParameter
            where TDbEx : DbException
        {
            return new Queryable<TCommand, TCon, TTrans, TParam, TDbEx>(
                new CommandEngine<TCommand, TCon, TDbEx>(GetConfiguration<TCommand>(connection)));
        }

        internal static IMultipleQueryable<TItem, TParam> NewMultiple<TItem, TCommand, TCon, TTrans, TParam, TDbEx>(
            string connection, IEnumerable<TItem> collection)
            where TCommand : DbCommand, new()
            where TCon : DbConnection, new()
            where TTrans : DbTransaction
            where TParam : DbParameter
            where TDbEx : DbException
        {
            return new MultipleQueryable<TItem, TCommand, TCon, TTrans, TParam, TDbEx>(
                new IteratingCommandEngine<TItem, TCommand, TCon, TDbEx>(
                    GetMultipleConfig<TItem, TCommand>(connection, collection)));
        }

        private static StaticCommandConfig GetConfiguration<TCommand>(string connection)
            where TCommand : DbCommand
        {
            var configuration = Configuration.GetCommandConfig<TCommand>();
            if (string.IsNullOrEmpty(connection))
            {
                return configuration;
            }
            var copy = configuration.Copy();
            copy.ConnectionString = connection;
            return copy;
        }

        private static IteratingCommandConfig<TItem> GetMultipleConfig<TItem, TCommand>(
            string connection, IEnumerable<TItem> collection) where TCommand : DbCommand
        {
            return new IteratingCommandConfig<TItem>()
            {
                Collection = collection,
                Static = GetConfiguration<TCommand>(connection)
            };
        }
    }
}
