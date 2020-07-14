using System.Data.Common;

namespace FluentDB.Factories
{
    public interface ICommandFactory
    {
        DbCommand Create();
    }
}