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

        public static Type ActiveCommandType { get; private set; }

        public static Configuration<TCommand> New<TCommand>()
            where TCommand : DbCommand
        {
            if (ActiveCommandType == null)
            {
                ActiveCommandType = typeof(TCommand);
            }
            return new Configuration<TCommand>(commandConfigurations[typeof(TCommand)]);
        }

        public static void SetActiveCommandTypeTo<TCommand>() where TCommand : DbCommand
        {
            ActiveCommandType = typeof(TCommand);
        }

        internal static StaticCommandConfig GetCommandConfig(Type commandType)
        {
            if (commandType == null)
            {
                commandType = ActiveCommandType ?? throw new ArgumentNullException(nameof(ActiveCommandType));
            }
            if (commandConfigurations.ContainsKey(commandType))
            {
                return commandConfigurations[commandType];
            }
            var newCommandConfig = new StaticCommandConfig();
            commandConfigurations.Add(commandType, newCommandConfig);
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
