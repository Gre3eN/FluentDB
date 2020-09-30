using FluentDB.Command;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace FluentDB.Services
{
    internal class ConfigurationBuilder
    {
        private StaticCommandConfig commandConfig;

        private ConfigurationBuilder(StaticCommandConfig commandConfig)
        {
            this.commandConfig = commandConfig ?? throw new ArgumentNullException(nameof(commandConfig));
        }

        internal static ConfigurationBuilder New(Type commandType)
        {
            return new ConfigurationBuilder(Configuration.GetCommandConfig(commandType));
        }

        internal ConfigurationBuilder WithDefaultConnection()
        {
            if (commandConfig.ConnectionString == null)
            {
                throw new ArgumentNullException(nameof(commandConfig.ConnectionString), "Default Connection not set!");
            }
            return this;
        }

        internal ConfigurationBuilder WithConnection(string connection)
        {
            commandConfig = commandConfig.Copy();
            commandConfig.ConnectionString = connection;
            return this;
        }

        internal ConfigurationBuilder WithTransaction(DbTransaction transaction)
        {
            commandConfig = commandConfig.Copy();
            commandConfig.Transaction = transaction;
            return this;
        }

        internal StaticCommandConfig Build()
        {
            if (commandConfig.Transaction == null && commandConfig.ConnectionString == null)
            {
                throw new ArgumentException("Unable to build Command Config! No transaction or connection string set!");
            }
            return commandConfig;
        }

        internal IteratingCommandConfig<TItem> BuildAsMultiple<TItem>(IEnumerable<TItem> collection)
        {
            return new IteratingCommandConfig<TItem>(Build(), collection);
        }
    }
}
