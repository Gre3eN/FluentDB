using FluentDB.Command;
using FluentDB.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace FluentDB.Services.Multiple
{
    internal class MultipleQueryable<TItem>
        : IMultipleQueryable<TItem>,
        IMultipleConfigureQueryable<TItem>,
        IFirstParameterQueryable<TItem>,
        IParameterQueryable<TItem>,
        IRunMultiple
    {
        private readonly IteratingCommandEngine<TItem> commandEngine;

        private string currentParamId;

        public Dictionary<string, DbParameter> Parameters { get; }

        internal MultipleQueryable(IteratingCommandEngine<TItem> commandEngine)
        {
            this.commandEngine = commandEngine ?? throw new ArgumentNullException(nameof(commandEngine));
            Parameters = new Dictionary<string, DbParameter>();
        }

        public IMultipleConfigureQueryable<TItem> For(string queryText)
        {
            commandEngine.AddConfiguration(command => command.CommandText = queryText);
            return this;
        }

        public IMultipleConfigureQueryable<TItem> CommandType(CommandType type)
        {
            commandEngine.AddConfiguration(command => command.CommandType = type);
            return this;
        }

        public IFirstParameterQueryable<TItem> WithParameters()
        {
            return this;
        }

        public IParameterQueryable<TItem> Parameter(string paramId, Func<TItem, object> createValue)
        {
            currentParamId = paramId;
            commandEngine.AddParamConfiguration((command, item) =>
            {
                var value = createValue(item);
                if (command.Parameters.Contains(paramId))
                {
                    command.Parameters[paramId].Value = value;
                    command.Parameters[paramId].DbType = DbTypeConverter.From(value.GetType());
                }
                else
                {
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = paramId;
                    parameter.Value = value;
                    parameter.DbType = DbTypeConverter.From(value.GetType());
                    command.Parameters.Add(parameter);
                }
            });
            return this;
        }

        public IParameterQueryable<TItem> ExplicitType(DbType type)
        {
            commandEngine.AddConfiguration(command => command.Parameters[currentParamId].DbType = type);
            return this;
        }

        public IParameterQueryable<TItem> Direction(ParameterDirection direction)
        {
            commandEngine.AddConfiguration(command => command.Parameters[currentParamId].Direction = direction);
            return this;
        }

        internal IParameterQueryable<TItem> ConfigureCurrentParameter<TParam>(Action<TParam> configure)
            where TParam : DbParameter
        {
            commandEngine.AddConfiguration(command => 
            {
                if (command.Parameters[currentParamId] is TParam param)
                {
                    configure(param);
                }
            });
            return this;
        }

        public IRunMultiple Run()
        {
            return this;
        }

        public void AsNonQuery()
        {
            commandEngine.Run(command => command.ExecuteNonQuery());
        }
    }
}
