
using Ramsha;
using Ramsha.Identity;
using Ramsha.Identity.Domain;
using Ramsha.Identity.Shared;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static RamshaBuilder AddIdentity(this RamshaBuilder ramsha)
    {
        ramsha.AddModule<IdentityModule>();
        return ramsha;
    }

    public static RamshaBuilder AddIdentity<TUser>(this RamshaBuilder ramsha)
        where TUser : RamshaIdentityUser
    {
        ramsha.AddModule<IdentityModule>();

        ramsha.PrepareOptions<RamshaIdentityTypesOptions>(options =>
        {
            options.UserType = typeof(TUser);
        });

        return ramsha;
    }

    public static RamshaBuilder AddIdentity<TUser, TRole>(this RamshaBuilder ramsha)
        where TUser : RamshaIdentityUser
        where TRole : RamshaIdentityRole
    {
        ramsha.AddModule<IdentityModule>();

        ramsha.PrepareOptions<RamshaIdentityTypesOptions>(options =>
        {
            options.UserType = typeof(TUser);
            options.RoleType = typeof(TRole);
        });

        return ramsha;
    }


    public static RamshaBuilder AddIdentity<TUser, TRole, TKey>(this RamshaBuilder ramsha)
     where TUser : RamshaIdentityUser<TKey>
     where TRole : RamshaIdentityRole<TKey>
        where TKey : IEquatable<TKey>
    {
        ramsha.AddModule<IdentityModule>();

        ramsha.PrepareOptions<RamshaIdentityTypesOptions>(options =>
        {
            options.UserType = typeof(TUser);
            options.RoleType = typeof(TRole);
        });

        return ramsha;
    }
}
