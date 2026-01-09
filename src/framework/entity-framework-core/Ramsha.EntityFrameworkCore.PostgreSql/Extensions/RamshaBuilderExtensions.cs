

using Ramsha.EntityFrameworkCore.PostgreSql;

namespace Ramsha;

public static class RamshaBuilderExtensions
{
    public static RamshaBuilder AddEFPostgreSqlModule(this RamshaBuilder ramsha)
    {
        ramsha.AddModule<EntityFrameworkCorePostgreSqlModule>();
        return ramsha;
    }
}
