using System.Data.Common;

namespace FluentDB.Factories
{
    public interface IConnectionFactory
    {
        DbConnection Create(string connection);
    }
}