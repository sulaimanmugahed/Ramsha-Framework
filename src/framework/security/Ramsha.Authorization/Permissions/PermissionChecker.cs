using System.Security.Claims;


namespace Ramsha.Authorization;

public class PermissionChecker(
        PermissionProviderResolversManager providerManager,
        IPermissionDefinitionStore definitionStore) : IPermissionChecker
{

    public Task<bool> IsAssignedAsync(string name)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsAssignedAsync(ClaimsPrincipal? claimsPrincipal, string permissionName)
    {
        var userId = claimsPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return false;


        var permission = await definitionStore.GetPermissionAsync(permissionName);
        if (permission is null)
            return false;


        var allowed = false;
        var context = new PermissionResolveContext(permission, claimsPrincipal);
        foreach (var provider in providerManager.PermissionResolvers)
        {
            if (ShouldSkipResolver(context.Permission.Providers, provider.GetProviderName()))
            {
                continue;
            }

            var result = await provider.ResolveAsync(context);

            if (result == PermissionStatus.Allow)
            {
                allowed = true;
            }
            else if (result == PermissionStatus.Deny)
            {
                return false;
            }
        }

        return allowed;
    }

    private bool ShouldSkipResolver(List<string> permissionAllowedProviders, string resolverProviderName)
    => permissionAllowedProviders.Any() &&
                !permissionAllowedProviders.Contains(resolverProviderName);



    private bool PermissionExists(IEnumerable<PermissionDefinition> permissions, string name)
    {
        foreach (var p in permissions)
        {
            if (p.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                return true;

            if (PermissionExists(p.Children, name))
                return true;
        }
        return false;
    }
}


