using System.Security.Principal;


namespace Ramsha.Authorization;

public class UserPermissionProviderResolver(IPermissionStore store) : IPermissionProviderResolver
{
    public string GetProviderName()
    {
        return "U";
    }

    public async Task<PermissionStatus> ResolveAsync(PermissionResolveContext context)
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


