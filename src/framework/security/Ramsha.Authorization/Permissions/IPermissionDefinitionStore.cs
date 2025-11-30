namespace Ramsha.Authorization;

public interface IPermissionDefinitionStore
{
    Task AddGroupAsync(PermissionGroupDefinition group);
    Task AddPermissionAsync(string groupName, PermissionDefinition permission);

    Task<IReadOnlyList<PermissionGroupDefinition>> GetGroupsAsync();
    Task<PermissionGroupDefinition?> GetGroupAsync(string groupName);

    Task<IReadOnlyList<PermissionDefinition>> GetAllPermissionsAsync();
    Task<PermissionDefinition?> GetAsync(string permissionName);

}


