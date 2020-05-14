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
        private readonly List<CommandConfiguration<TCommand>> configurations;

        public IteratingCommandEngine(IteratingCommandConfig<TItem> commandConfig)
        {
            this.commandConfig = commandConfig ?? throw new ArgumentNullException(nameof(commandConfig));
            this.configurations = new List<CommandConfiguration<TCommand>>();
        }

        public void AddConfiguration(CommandConfiguration<TCommand> configuration)
        {
            configurations.Add(configuration);
        }

        public void Run(Action<TCommand> commandAction)
        {
            try
            {
                using var command = new TCommand();
                configurations.ForEach(c => c.Execute(command));
                using var connection = command.Connection
                    ?? new TCon { ConnectionString = commandConfig.Static.ConnectionString };
                connection.Open();
                foreach (var item in commandConfig.Collection)
                {
                    configurations.ForEach(c => c.Execute(command, item));
                    commandAction(command);
                }
            }
            catch (TDbEx ex)
            {
                //TODO
            }
        }

        public T Run<T>(Func<TCommand, T> commandFunction)
        {
            throw new NotImplementedException();
        }
    }
}
