using FluentDB.Command;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace FluentDB.Services.Multiple
{
    public class MultipleQueryable<TItem, TCommand, TCon, TTrans, TParam, TDbEx>
        : IMultipleQueryable<TItem, TParam>,
        IMultipleConfigureQueryable<TItem, TParam>,
        IMultipleParameterQueryable<TItem, TParam>,
        IRunMultiple
        where TCommand : DbCommand, new()
        where TCon : DbConnection, new()
        where TTrans : DbTransaction
        where TParam : DbParameter
        where TDbEx : DbException
    {
        private readonly IteratingCommandEngine<TItem, TCommand, TCon, TDbEx> commandEngine;

        public Dictionary<string, DbParameter> Parameters { get; }

        public MultipleQueryable(IteratingCommandEngine<TItem, TCommand, TCon, TDbEx> commandEngine)
        {
            this.commandEngine = commandEngine ?? throw new ArgumentNullException(nameof(commandEngine));
            Parameters = new Dictionary<string, DbParameter>();
        }

        public void AsNonQuery()
        {
            commandEngine.Run(command => command.ExecuteNonQuery());
        }

        public IMultipleConfigureQueryable<TItem, TParam> CommandType(CommandType type)
        {
            commandEngine.AddConfiguration(command => command.CommandType = type);
            return this;
        }

        public IMultipleConfigureQueryable<TItem, TParam> For(string queryText)
        {
            commandEngine.AddConfiguration(command => command.CommandText = queryText);
            return this;
        }

        public IMultipleParameterQueryable<TItem, TParam> Parameter(string paramId, Action<TParam, TItem> configure)
        {
            commandEngine.AddParamConfiguration((command, item) =>
            {
                if (command.Parameters.Contains(paramId))
                {
                    configure((TParam)command.Parameters[paramId], item);
                }
                else
                {
                    var parameter = command.CreateParameter();
                    configure((TParam)parameter, item);
                    command.Parameters.Add(parameter);
                }
            });
            return this;
        }

        public IRunMultiple Run()
        {
            throw new NotImplementedException();
        }

        public IMultipleParameterQueryable<TItem, TParam> With()
        {
            return this;
        }
    }
}
