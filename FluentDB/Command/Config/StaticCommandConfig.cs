using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace FluentDB.Command
{
    public class StaticCommandConfig
    {
        public Type CommandType { get; }
        public DbTransaction Transaction { get; internal set; }
        public string ConnectionString { get; internal set; }

        public StaticCommandConfig(Type commandType)
        {
            CommandType = commandType ?? throw new ArgumentNullException(nameof(commandType));
        }

        public StaticCommandConfig Copy()
        {
            return new StaticCommandConfig(CommandType)
            {
                Transaction = Transaction,
                ConnectionString = ConnectionString
            };
        }
    }
}
