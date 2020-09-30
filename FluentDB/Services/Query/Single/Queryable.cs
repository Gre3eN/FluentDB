using FluentDB.Command;
using FluentDB.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace FluentDB.Services
{
    internal class Queryable
        : INewQueryable, 
        IConfigureQueryable,
        IFirstParameterQueryable,
        IParameterQueryable,
        IResult
    {
        private readonly CommandEngine commandEngine;

        private string currentParamId;

        public Dictionary<string, DbParameter> Parameters { get; }

        internal Queryable(CommandEngine commandEngine)
        {
            this.commandEngine = commandEngine ?? throw new ArgumentNullException(nameof(commandEngine));
            Parameters = new Dictionary<string, DbParameter>();
        }

        public IConfigureQueryable For(string queryText)
        {
            commandEngine.AddConfiguration(command => command.CommandText = queryText);
            return this;
        }

        public IConfigureQueryable CommandType(CommandType type)
        {
            commandEngine.AddConfiguration(command => command.CommandType = type);
            return this;
        }

        public IFirstParameterQueryable WithParameters()
        {
            return this;
        }

        public IParameterQueryable Parameter(string paramId, object value)
        {
            currentParamId = paramId;

            commandEngine.AddConfiguration(command =>
            {
                var param = command.CreateParameter();
                param.ParameterName = paramId;
                param.Value = value;
                param.DbType = DbTypeConverter.From(value.GetType());
                command.Parameters.Add(param);
                Parameters.Add(param.ParameterName, param);
            });
            return this;
        }

        public IParameterQueryable ExplicitType(DbType type)
        {
            commandEngine.AddConfiguration(command => command.Parameters[currentParamId].DbType = type);
            return this;
        }

        public IParameterQueryable Direction(ParameterDirection direction)
        {
            commandEngine.AddConfiguration(command => command.Parameters[currentParamId].Direction = direction);
            return this;
        }

        internal IParameterQueryable ConfigureCurrentParameter<TParam>(Action<TParam> configure)
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

        public IResult Run()
        {
            return this;
        }

        public T As<T>(Func<DbDataReader, T> read)
        {
            return commandEngine.Run(command =>
            {
                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return read(reader);
                }
                throw new ArgumentException("Empty result");
            });
        }

        public IEnumerable<T> AsEnumerable<T>(Func<DbDataReader, T> read)
        {
            return commandEngine.Run(command => ReadEnumerable(command, read));
        }

        private IEnumerable<T> ReadEnumerable<T>(DbCommand command, Func<DbDataReader, T> read)
        {
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                yield return read(reader);
            }
        }

        public void AsNonQuery()
        {
            commandEngine.Run(command => command.ExecuteNonQuery());
        }

        public T AsScalar<T>(Func<object, T> convert)
        {
            return commandEngine.Run(command => convert(command.ExecuteScalar()));
        }
    }
}
