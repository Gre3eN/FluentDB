using System;
using System.Collections.Generic;
using System.Text;

namespace FluentDB.Services.Multiple
{
    public interface IFirstParameterQueryable<TItem>
    {
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
