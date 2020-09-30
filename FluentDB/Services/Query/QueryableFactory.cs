using FluentDB.Command;
using FluentDB.Factories;
using FluentDB.Services.Multiple;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace FluentDB.Services
{
    internal static class QueryableFactory
    {
        internal static INewQueryable New(StaticCommandConfig commandConfig)
        {
            return new Queryable(new CommandEngine(commandConfig));
        }

        internal static IMultipleQueryable<TItem> NewMultiple<TItem>(IteratingCommandConfig<TItem> commandConfig)
        {
            return new MultipleQueryable<TItem>(new IteratingCommandEngine<TItem>(commandConfig));
        }
    }
}
