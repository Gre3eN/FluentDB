using FluentDB.Command;
using FluentDB.Factories;
using FluentDB.Services.Multiple;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace FluentDB.Services
{
    internal static class QueryableFactory
    {
        internal static INewQueryable New(Type commandType, string connection = null)
        {
            return new Queryable(new CommandEngine(
                CommandFactory.New(commandType),
                ConnectionFactory.New(commandType),
                GetConfiguration(commandType, connection)
                ));
        }

        internal static IMultipleQueryable<TItem> NewMultiple<TItem>(
            IEnumerable<TItem> collection, Type commandType, string connection = null)
        {
            return new MultipleQueryable<TItem>(new IteratingCommandEngine<TItem>(
                CommandFactory.New(commandType),
                ConnectionFactory.New(commandType),
                GetMultipleConfig(commandType, collection, connection)
                ));
        }

        private static StaticCommandConfig GetConfiguration(Type commandType, string connection)
        {
            var configuration = Configuration.GetCommandConfig(commandType);
            if (string.IsNullOrEmpty(connection))
            {
                return configuration;
            }
            var copy = configuration.Copy();
            copy.ConnectionString = connection;
            return copy;
        }

        private static IteratingCommandConfig<TItem> GetMultipleConfig<TItem>(
            Type commandType, IEnumerable<TItem> collection, string connection)
        {
            return new IteratingCommandConfig<TItem>()
            {
                Collection = collection,
                Static = GetConfiguration(commandType, connection)
            };
        }
    }
}
