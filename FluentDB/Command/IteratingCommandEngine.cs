using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace FluentDB.Command
{
    public class IteratingCommandEngine<TItem, TCommand, TCon, TDbEx> : ICommandEngine<TCommand>
        where TCommand : DbCommand, new()
        where TCon : DbConnection, new()
        where TDbEx : DbException
    {
        private readonly IteratingCommandConfig<TItem> commandConfig;
        private readonly List<Action<TCommand>> configurations;
        private readonly List<Action<TCommand, TItem>> paramConfiguration;

        public IteratingCommandEngine(IteratingCommandConfig<TItem> commandConfig)
        {
            this.commandConfig = commandConfig ?? throw new ArgumentNullException(nameof(commandConfig));
            configurations = new List<Action<TCommand>>();
            paramConfiguration = new List<Action<TCommand, TItem>>();
        }

        public void AddConfiguration(Action<TCommand> configuration)
        {
            configurations.Add(configuration);
        }

        public void AddParamConfiguration(Action<TCommand, TItem> setParameter)
        {
            paramConfiguration.Add(setParameter);
        }

        public void Run(Action<TCommand> commandAction)
        {
            try
            {
                using var command = new TCommand();
                configurations.ForEach(c => c(command));
                using var connection = command.Connection
                    ?? new TCon { ConnectionString = commandConfig.Static.ConnectionString };
                connection.Open();
                foreach (var item in commandConfig.Collection)
                {
                    paramConfiguration.ForEach(c => c(command, item));
                    commandAction(command);
                }
            }
            catch (TDbEx ex)
            {
                throw ex;
                //TODO
            }
        }
    }
}
