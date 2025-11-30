namespace Ramsha.Authorization;

public interface IPermissionStore
{
    Task<bool> HasPermissionAsync(
        string permissionName,
         string providerName,
         string providerKey);

    Task GrantPermissionAsync(
        string permissionName,
        string providerName,
        string providerKey,
        bool isGrant);
}


