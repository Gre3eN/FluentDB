using System;
using System.Data;

namespace FluentDB.Services.Multiple
{
    public interface IParameterQueryable<TItem>
    {
        IParameterQueryable<TItem> Direction(ParameterDirection direction);
        IParameterQueryable<TItem> ExplicitType(DbType type);
        IParameterQueryable<TItem> Parameter(string paramId, Func<TItem, object> createValue);
        IRunMultiple Run();
    }
}