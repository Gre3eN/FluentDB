using System;
using System.Collections.Generic;
using System.Text;

namespace FluentDB.Command
{
    public class IteratingCommandConfig<T>
    {
        public StaticCommandConfig Static { get; }
        public IEnumerable<T> Collection { get; }

        public IteratingCommandConfig(StaticCommandConfig staticCommandConfig, IEnumerable<T> collection)
        {
            Static = staticCommandConfig ?? throw new ArgumentNullException(nameof(staticCommandConfig));
            Collection = collection ?? throw new ArgumentNullException(nameof(collection));
        }
    }
}
