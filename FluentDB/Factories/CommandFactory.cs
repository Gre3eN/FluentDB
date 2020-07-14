using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace FluentDB.Factories
{
    public static class CommandFactory
    {
        private static readonly Dictionary<Type, ICommandFactory> factoryTypeMap = new Dictionary<Type, ICommandFactory>
        {
            { typeof(SqlCommand), new CommandFactory<SqlCommand>() }
        };

        public static ICommandFactory New(Type commandType)
        {
            return factoryTypeMap[commandType];
        }
    }

    public class CommandFactory<TCommand> : ICommandFactory 
        where TCommand : DbCommand, new()
    {
        public DbCommand Create()
        {
            return new TCommand();
        }
    }
}
