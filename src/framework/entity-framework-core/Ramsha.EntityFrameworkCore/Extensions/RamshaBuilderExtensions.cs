

using Ramsha.EntityFrameworkCore;

namespace Ramsha;

public static class RamshaBuilderExtensions
{
    public static RamshaBuilder AddEntityFrameworkCoreModule(this RamshaBuilder ramsha)
    {
        ramsha.AddModule<EntityFrameworkCoreModule>();
        return ramsha;
    }
}
