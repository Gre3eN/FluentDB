using System;
using System.Collections.Generic;
using System.Text;

namespace FluentDB.Services.Multiple
{
    public interface IFirstParameterQueryable<TItem>
    {
        IParameterQueryable<TItem> Parameter(string paramId, Func<TItem, object> createValue);
    }
}
