using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace FluentDB.Services
{
    public interface INewQuerable<TParam> where TParam : DbParameter
    {
        IConfigureQuerable<TParam> For(string queryText);
    }
}
