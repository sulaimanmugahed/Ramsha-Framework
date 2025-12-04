namespace Ramsha.Authorization;

public interface IPermissionDefinitionStore
{
    Task<IReadOnlyList<PermissionGroupDefinition>> GetGroupsAsync();
    Task<PermissionGroupDefinition?> GetGroupAsync(string groupName);
    Task<IReadOnlyList<PermissionDefinition>> GetPermissionsAsync();
    Task<PermissionDefinition?> GetPermissionAsync(string permissionName);

}




