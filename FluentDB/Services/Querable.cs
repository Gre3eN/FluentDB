using FluentDB.Command;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace FluentDB.Services
{
    public class Querable<TCommand, TCon, TTrans, TParam, TDbEx> 
        : INewQuerable<TParam>, 
        IMultipleQueryable<TParam>,
        IConfigureQuerable<TParam>,
        IMultipleConfigureQueryable<TParam>,
        ISingleParameterQuerable<TParam>,
        IMultipleParameterQuerable<TParam>,
        IResult,
        IRunMultiple
        where TCommand : DbCommand, new()
        where TCon : DbConnection, new()
        where TTrans : DbTransaction
        where TParam : DbParameter
        where TDbEx : DbException
    {
        private readonly CommandEngine<TCommand, TCon, TDbEx> commandEngine;

        public Dictionary<string, DbParameter> Parameters { get; }

        public Querable(CommandEngine<TCommand, TCon, TDbEx> commandEngine)
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

        public ISingleParameterQuerable<TParam> With()
        {
            return this;
        }

        public IConfigureQuerable<TParam> CommandType(CommandType type)
        {
            commandEngine.AddConfiguration(new CommandConfiguration<TCommand>(
                command => command.CommandType = type));
            return this;
        }

        public IConfigureQuerable<TParam> For(string queryText)
        {
            commandEngine.AddConfiguration(new CommandConfiguration<TCommand>(
                command => command.CommandText = queryText));
            return this;
        }

        IMultipleConfigureQueryable<TParam> IMultipleQueryable<TParam>.For(string queryText)
        {
            commandEngine.AddConfiguration(new CommandConfiguration<TCommand>(
                command => command.CommandText = queryText));
            return this;
        }

        public ISingleParameterQuerable<TParam> Parameter(Action<TParam> configure)
        {
            commandEngine.AddConfiguration(new CommandConfiguration<TCommand>(command => 
            {
                var param = (TParam)command.CreateParameter();
                configure(param);
                command.Parameters.Add(param);
                Parameters.Add(param.ParameterName, param);
            }));
            return this;
        }

        public IResult Run()
        {
            return this;
        }

        IMultipleConfigureQueryable<TParam> IMultipleConfigureQueryable<TParam>.CommandType(CommandType type)
        {
            commandEngine.AddConfiguration(new CommandConfiguration<TCommand>(
                command => command.CommandType = type));
            return this;
        }

        IMultipleParameterQuerable<TParam> IMultipleConfigureQueryable<TParam>.With()
        {
            return this;
        }

        public IMultipleParameterQuerable<TParam> Parameter(Action<TParam, object> configure)
        {
            commandEngine.AddConfiguration(new CommandConfiguration<TCommand>((command, obj) =>
            {
                var param = (TParam)command.CreateParameter();
                configure(param, obj);
                if (command.Parameters.Contains(param.ParameterName))
                {
                    command.Parameters[param.ParameterName] = param;
                }
                else
                {
                    command.Parameters.Add(param);
                    Parameters.Add(param.ParameterName, param);
                }
            }));
            return this;
        }

        IRunMultiple IMultipleConfigureQueryable<TParam>.Run()
        {
            return this;
        }

        IRunMultiple IMultipleParameterQuerable<TParam>.Run()
        {
            return this;
        }
    }
}
