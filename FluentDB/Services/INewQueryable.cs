using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace FluentDB.Services
{
    public interface INewQueryable
    {
        IConfigureQueryable For(string queryText);
    }
}
