namespace Ramsha.Authorization;

public class InMemoryPermissionStore : IPermissionStore
{
    private readonly Dictionary<(string ProviderName, string ProviderKey), HashSet<string>> _permissions =
            new();



    public Task<bool> IsAssignedAsync(
        string permissionName,
        string providerName,
        string providerKey)
    {
        var key = (providerName, providerKey);

        if (_permissions.TryGetValue(key, out var set))
            return Task.FromResult(set.Contains(permissionName));

        return Task.FromResult(false);
    }
}


