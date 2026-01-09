

using Ramsha.EntityFrameworkCore.SqlServer;

namespace Ramsha;

public static class RamshaBuilderExtensions
{
    public static RamshaBuilder AddEFSqlServerModule(this RamshaBuilder ramsha)
    {
        ramsha.AddModule<EntityFrameworkCoreSqlServerModule>();
        return ramsha;
    }
}
