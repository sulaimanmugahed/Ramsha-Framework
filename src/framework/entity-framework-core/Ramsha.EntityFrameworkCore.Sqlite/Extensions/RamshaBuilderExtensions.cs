

using Ramsha.EntityFrameworkCore.Sqlite;

namespace Ramsha;

public static class RamshaBuilderExtensions
{
    public static RamshaBuilder AddEFSqliteModule(this RamshaBuilder ramsha)
    {
        ramsha.AddModule<EntityFrameworkCoreSqliteModule>();
        return ramsha;
    }
}
