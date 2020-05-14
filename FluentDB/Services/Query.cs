using FluentDB.Command;
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
            GetCommandConfig().ConnectionString = connectionString;
        }

        public static Query<TCommand> New()
        {
            return new Query<TCommand>(GetCommandConfig());
        }

        internal static Query<TCommand> New<T>(IEnumerable<T> collection)
        {

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

        private readonly StaticCommandConfig commandConfig;

        private Query(StaticCommandConfig commandConfig)
        {
            this.commandConfig = commandConfig;
        }

        internal INewQuerable<TParam> WithDefaultConnection<TCon, TTrans, TParam, TDbEx>() 
            where TCon : DbConnection, new()
            where TTrans : DbTransaction
            where TParam : DbParameter
            where TDbEx : DbException
        {
            return new Querable<TCommand, TCon, TTrans, TParam, TDbEx>(
                new CommandEngine<TCommand, TCon, TDbEx>(commandConfig));
        }
    }
}
