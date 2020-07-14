using System;
using System.Collections.Generic;
using System.Text;

namespace FluentDB.Command
{
    public class StaticCommandConfig
    {
        public string ConnectionString { get; internal set; }

        public StaticCommandConfig(string connectionString)
        {
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public StaticCommandConfig Copy()
        {
            return new StaticCommandConfig(ConnectionString);
        }
    }
}
