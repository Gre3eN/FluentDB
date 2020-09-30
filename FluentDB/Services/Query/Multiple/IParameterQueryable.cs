using System;
using System.Data;

namespace FluentDB.Services.Multiple
{
    public interface IParameterQueryable<TItem>
    {
        /// <summary>
        /// Sets the direction of the parameter. Default is Input.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        IParameterQueryable<TItem> Direction(ParameterDirection direction);
        /// <summary>
        /// Sets the type of the parameter explicitly.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IParameterQueryable<TItem> ExplicitType(DbType type);
        /// <summary>
        /// Configures a parameter with the given id. The function to create the value 
        /// will be called on every item in the collection.
        /// </summary>
        /// <param name="paramId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IParameterQueryable<TItem> Parameter(string paramId, Func<TItem, object> createValue);
        /// <summary>
        /// Allows to run the query.
        /// </summary>
        /// <returns></returns>
        IRunMultiple Run();
    }
}