using System;
using System.Collections.Generic;
using System.Text;

namespace FluentDB.Command
{
    public class StaticCommandConfig
    {
        public string ConnectionString { get; set; }

        public StaticCommandConfig Copy()
        {
            return new StaticCommandConfig { ConnectionString = ConnectionString };
        }
    }
}
