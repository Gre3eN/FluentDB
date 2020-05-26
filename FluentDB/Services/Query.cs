using FluentDB.Command;
using FluentDB.Services.Multiple;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace FluentDB.Services
{
    public class Query<TCommand> where TCommand : DbCommand, new()
    {
        #region static
        private static readonly Dictionary<Type, StaticCommandConfig> commandConfigurations
            = new Dictionary<Type, StaticCommandConfig>();

        public static void ConfigureConnection(string connectionString)
        {
            GetCommandConfig().ConnectionString = connectionString 
                ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public static Query<TCommand> New()
        {
            return new Query<TCommand>();
        }

        private static StaticCommandConfig GetCommandConfig()
        {
            if (commandConfigurations.ContainsKey(typeof(TCommand)))
            {
                return commandConfigurations[typeof(TCommand)];
            }
            var newCommandConfig = new StaticCommandConfig();
            commandConfigurations.Add(typeof(TCommand), newCommandConfig);
            return newCommandConfig;
        }

        #endregion static

        internal INewQueryable<TParam> New<TCon, TTrans, TParam, TDbEx>(string connection = null)
            where TCon : DbConnection, new()
            where TTrans : DbTransaction
            where TParam : DbParameter
            where TDbEx : DbException
        {
            if (connection != null)
            {
                ConfigureConnection(connection);
            }
            return new Queryable<TCommand, TCon, TTrans, TParam, TDbEx>(
                new CommandEngine<TCommand, TCon, TDbEx>(GetCommandConfig()));
        }

        internal IMultipleQueryable<TParam> NewMultiple<TItem, TCon, TTrans, TParam, TDbEx>(
            IEnumerable<TItem> collection,
            string connection = null)
            where TCon : DbConnection, new()
            where TTrans : DbTransaction
            where TParam : DbParameter
            where TDbEx : DbException
        {
            if (connection != null)
            {
                ConfigureConnection(connection);
            }
            var configuration = new IteratingCommandConfig<TItem>
            {
                Static = GetCommandConfig(),
                Collection = collection
            };
            return new MultipleQueryable<TItem, TCommand, TCon, TTrans, TParam, TDbEx>(
                new IteratingCommandEngine<TItem, TCommand, TCon, TDbEx>(configuration));
        }
    }
}
