using FluentDB.Command;
using FluentDB.Factories;
using FluentDB.Services.Multiple;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace FluentDB.Services
{
    public class Query
    {
        private readonly Type optionalCommandType;

        public Query()
        {
            optionalCommandType = null;
        }

        public Query(Type optionalCommandType)
        {
            this.optionalCommandType = optionalCommandType;
        }

        public static Query New()
        {
            return new Query();
        }

        public static Query New<TCommand>() where TCommand : DbCommand, new()
        {
            return new Query(typeof(TCommand));
        }

        public INewQueryable WithDefaultConnection()
        {
            return QueryableFactory.New(optionalCommandType);
        }

        public INewQueryable With(string connectionString)
        {
            return QueryableFactory.New(optionalCommandType, connectionString);
        }
    }
}
