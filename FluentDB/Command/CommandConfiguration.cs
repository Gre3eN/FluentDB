using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace FluentDB.Command
{
    public class CommandConfiguration<TCommand> where TCommand : DbCommand
    {
        private readonly Action<TCommand> normal;
        private readonly Action<TCommand, object> forMultipleParams;

        public CommandConfiguration(Action<TCommand> normal)
        {
            this.normal = normal ?? throw new ArgumentNullException(nameof(normal));
        }

        public CommandConfiguration(Action<TCommand, object> forMultipleParams)
        {
            this.forMultipleParams = forMultipleParams ?? throw new ArgumentNullException(nameof(forMultipleParams));
        }

        public void Execute(TCommand command)
        {
            normal?.Invoke(command);
        }

        public void Execute(TCommand command, object paramValue)
        {
            forMultipleParams?.Invoke(command, paramValue);
        }
    }
}
