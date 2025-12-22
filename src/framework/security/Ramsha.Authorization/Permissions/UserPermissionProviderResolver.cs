using System.Security.Principal;


namespace Ramsha.Authorization;

public class UserPermissionValueResolver(IPermissionStore store)
 : PermissionValueResolver("U")
{

    public override async Task<PermissionStatus> ResolveAsync(PermissionResolveContext context)
    {
        var userId = context.Principal?.FindUserId();
        if (userId is null)
        {
            return PermissionStatus.None;
        }

        var result = await store.IsAssignedAsync(context.Permission.Name, GetProviderName(), userId);
        return result ? PermissionStatus.Allow : PermissionStatus.None;
    }
}


