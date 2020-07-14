using FluentDB.Factories;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace FluentDB.Command
{
    public class IteratingCommandEngine<TItem> : ICommandEngine
    {
        private readonly ICommandFactory commandFactory;
        private readonly IConnectionFactory connectionFactory;
        private readonly IteratingCommandConfig<TItem> commandConfig;
        private readonly List<Action<DbCommand>> configurations;
        private readonly List<Action<DbCommand, TItem>> paramConfiguration;

        public IteratingCommandEngine(ICommandFactory commandFactory, 
            IConnectionFactory connectionFactory, 
            IteratingCommandConfig<TItem> commandConfig)
        {
            this.commandFactory = commandFactory ?? throw new ArgumentNullException(nameof(commandFactory));
            this.connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            this.commandConfig = commandConfig ?? throw new ArgumentNullException(nameof(commandConfig));
            configurations = new List<Action<DbCommand>>();
            paramConfiguration = new List<Action<DbCommand, TItem>>();
        }

        public void AddConfiguration(Action<DbCommand> configuration)
        {
            configurations.Add(configuration);
        }

        public void AddParamConfiguration(Action<DbCommand, TItem> setParameter)
        {
            paramConfiguration.Add(setParameter);
        }

        public void Run(Action<DbCommand> commandAction)
        {
            try
            {
                using var command = commandFactory.Create();
                configurations.ForEach(c => c(command));
                using var connection = command.Connection
                    ?? connectionFactory.Create(commandConfig.Static.ConnectionString); 
                connection.Open();
                foreach (var item in commandConfig.Collection)
                {
                    paramConfiguration.ForEach(c => c(command, item));
                    commandAction(command);
                }
            }
            catch (DbException ex)
            {
                throw ex;
                //TODO
            }
        }
    }
}
