using FluentDB.Command;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace FluentDB.Services
{
    public class Queryable<TCommand, TCon, TTrans, TParam, TDbEx> 
        : INewQueryable<TParam>, 
        IConfigureQueryable<TParam>,
        ISingleParameterQueryable<TParam>,
        IResult
        where TCommand : DbCommand, new()
        where TCon : DbConnection, new()
        where TTrans : DbTransaction
        where TParam : DbParameter
        where TDbEx : DbException
    {
        private readonly CommandEngine<TCommand, TCon, TDbEx> commandEngine;

        public Dictionary<string, DbParameter> Parameters { get; }

        public Queryable(CommandEngine<TCommand, TCon, TDbEx> commandEngine)
        {
            this.commandEngine = commandEngine ?? throw new ArgumentNullException(nameof(commandEngine));
            Parameters = new Dictionary<string, DbParameter>();
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

        private IEnumerable<T> ReadEnumerable<T>(TCommand command, Func<DbDataReader, T> read)
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

        public ISingleParameterQueryable<TParam> With()
        {
            return this;
        }

        public IConfigureQueryable<TParam> CommandType(CommandType type)
        {
            commandEngine.AddConfiguration(command => command.CommandType = type);
            return this;
        }

        public IConfigureQueryable<TParam> For(string queryText)
        {
            commandEngine.AddConfiguration(command => command.CommandText = queryText);
            return this;
        }

        public ISingleParameterQueryable<TParam> Parameter(Action<TParam> configure)
        {
            commandEngine.AddConfiguration(command => 
            {
                var param = (TParam)command.CreateParameter();
                configure(param);
                command.Parameters.Add(param);
                Parameters.Add(param.ParameterName, param);
            });
            return this;
        }

        public IResult Run()
        {
            return this;
        }
    }
}
