using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace FluentDB.Services
{
    public class Querable<TCommand, TCon, TTrans, TParam> 
        : INewQuerable<TParam>, 
        IConfigureQuerable<TParam>,
        ISingleParameterQuerable<TParam>,
        IResult
        where TCommand : DbCommand
        where TCon : DbConnection
        where TTrans : DbTransaction
        where TParam : DbParameter
    {
        private readonly TCommand command;

        internal Querable(TCommand command)
        {
            this.command = command ?? throw new ArgumentNullException(nameof(command));
        }

        public DbParameterCollection Parameters => command.Parameters;

        public T As<T>(Func<DbDataReader, T> read)
        {
            using var command = this.command;
            using var connection = command.Connection;
            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return read(reader);
            }
            throw new ArgumentException("Empty result");
        }

        public IEnumerable<T> AsEnumerable<T>(Func<DbDataReader, T> read)
        {
            using var command = this.command;
            using var connection = command.Connection;
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                yield return read(reader);
            }
        }

        public void AsNonQuery()
        {
            using var command = this.command;
            using var connection = command.Connection;
            connection.Open();
            command.ExecuteNonQuery();
        }

        public T AsScalar<T>(Func<object, T> convert)
        {
            using var command = this.command;
            using var connection = command.Connection;
            connection.Open();
            return convert(command.ExecuteScalar());
        }

        public ISingleParameterQuerable<TParam> AsSingle()
        {
            return this;
        }

        public IConfigureQuerable<TParam> CommandType(CommandType type)
        {
            command.CommandType = type;
            return this;
        }

        public IConfigureQuerable<TParam> For(string queryText)
        {
            command.CommandText = queryText;
            return this;
        }

        public ISingleParameterQuerable<TParam> Parameter(Action<TParam> configure)
        {
            var param = (TParam)command.CreateParameter();
            configure(param);
            command.Parameters.Add(param);
            return this;
        }

        public IResult Run()
        {
            return this;
        }
    }
}
