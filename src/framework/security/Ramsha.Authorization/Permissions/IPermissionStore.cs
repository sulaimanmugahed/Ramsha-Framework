namespace Ramsha.Authorization;

public interface IPermissionStore
{
    Task<bool> IsAssignedAsync(
        string permissionName,
         string providerName,
         string providerKey);
}


