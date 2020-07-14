using System;

namespace FluentDB.Services
{
    public interface IQuery
    {
        Type CommandType { get; }
    }
}