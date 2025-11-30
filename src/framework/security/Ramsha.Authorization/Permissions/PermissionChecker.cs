using System.Security.Claims;


namespace Ramsha.Authorization;

public class PermissionChecker(
        PermissionProviderResolversManager providerManager,
        IPermissionDefinitionStore definitionStore) : IPermissionChecker
{

    public Task<bool> HasPermissionAsync(string name)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> HasPermissionAsync(ClaimsPrincipal? claimsPrincipal, string permissionName)
    {
        var userId = claimsPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return false;


        var permission = await definitionStore.GetAsync(permissionName);
        if (permission is null)
            return false;


        var allowed = false;
        var context = new PermissionResolveContext(permission, claimsPrincipal);
        foreach (var provider in providerManager.PermissionResolvers)
        {
            if (ShouldSkipProvider(context.Permission.Providers, provider.GetProviderName()))
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

    private bool ShouldSkipProvider(List<string> permissionAllowedProviders, string providerName)
    => permissionAllowedProviders.Any() &&
                !permissionAllowedProviders.Contains(providerName);



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


