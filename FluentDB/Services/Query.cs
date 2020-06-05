using FluentDB.Command;
using FluentDB.Services.Multiple;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace FluentDB.Services
{
    public class Query<TCommand> where TCommand : DbCommand, new()
    {
        public static Query<TCommand> New()
        {
            return new Query<TCommand>();
        }
    }
}
