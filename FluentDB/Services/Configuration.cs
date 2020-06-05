using FluentDB.Command;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace FluentDB.Services
{
    public static class Configuration
    {
        private static readonly Dictionary<Type, StaticCommandConfig> commandConfigurations
            = new Dictionary<Type, StaticCommandConfig>();

        public static Configuration<TCommand> For<TCommand>()
            where TCommand : DbCommand
        {
            return new Configuration<TCommand>(commandConfigurations[typeof(TCommand)]);
        }

        internal static StaticCommandConfig GetCommandConfig<TCommand>()
            where TCommand : DbCommand
        {
            if (commandConfigurations.ContainsKey(typeof(TCommand)))
            {
                return commandConfigurations[typeof(TCommand)];
            }
            var newCommandConfig = new StaticCommandConfig();
            commandConfigurations.Add(typeof(TCommand), newCommandConfig);
            return newCommandConfig;
        }
    }

    public class Configuration<TCommand> where TCommand : DbCommand
    {
        private readonly StaticCommandConfig commandConfig;

        public Configuration(StaticCommandConfig commandConfig)
        {
            this.commandConfig = commandConfig ?? throw new ArgumentNullException(nameof(commandConfig));
        }

        public void ConfigureConnection(string connectionString)
        {
            commandConfig.ConnectionString = connectionString
                ?? throw new ArgumentNullException(nameof(connectionString));
        }
    }
}
