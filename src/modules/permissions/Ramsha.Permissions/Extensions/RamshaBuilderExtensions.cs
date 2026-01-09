using Ramsha.Permissions;

namespace Ramsha;

public static class RamshaBuilderExtensions
{
    public static RamshaBuilder AddPermissionsModule(this RamshaBuilder ramsha)
    {
        ramsha.AddModule<PermissionsModule>();
        return ramsha;
    }

}