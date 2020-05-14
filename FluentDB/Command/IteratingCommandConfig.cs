using System;
using System.Collections.Generic;
using System.Text;

namespace FluentDB.Command
{
    public class IteratingCommandConfig<T>
    {
        public StaticCommandConfig Static { get; set; }
        public IEnumerable<T> Collection { get; set; }
    }
}
