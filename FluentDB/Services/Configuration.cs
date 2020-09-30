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

        internal static Type ActiveCommandType { get; private set; }

        /// <summary>
        /// Create a new configuration for the given <typeparamref name="TCommand"/>.
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <returns></returns>
        public static Configuration<TCommand> New<TCommand>()
            where TCommand : DbCommand
        {
            if (ActiveCommandType == null)
            {
                ActiveCommandType = typeof(TCommand);
            }
            return new Configuration<TCommand>(GetCommandConfig(ActiveCommandType));
        }

        /// <summary>
        /// Sets the currently active command type to be used with queries and transactions which don't explicitly
        /// declare a command type to be used themselves.
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        public static void SetActiveCommandTypeTo<TCommand>() where TCommand : DbCommand
        {
            ActiveCommandType = typeof(TCommand);
        }

        internal static StaticCommandConfig GetCommandConfig(Type commandType)
        {
            if (commandConfigurations.ContainsKey(commandType))
            {
                return commandConfigurations[commandType];
            }
            var newCommandConfig = new StaticCommandConfig(commandType);
            commandConfigurations.Add(commandType, newCommandConfig);
            return newCommandConfig;
        }
    }

    public class Configuration<TCommand> where TCommand : DbCommand
    {
        private readonly StaticCommandConfig commandConfig;

        internal Configuration(StaticCommandConfig commandConfig)
        {
            this.commandConfig = commandConfig ?? throw new ArgumentNullException(nameof(commandConfig));
        }

        /// <summary>
        /// Configures a default connection for the given <typeparamref name="TCommand"/>.
        /// </summary>
        /// <param name="connectionString"></param>
        public void ConfigureDefaultConnection(string connectionString)
        {
            commandConfig.ConnectionString = connectionString
                ?? throw new ArgumentNullException(nameof(connectionString));
        }
    }
}
