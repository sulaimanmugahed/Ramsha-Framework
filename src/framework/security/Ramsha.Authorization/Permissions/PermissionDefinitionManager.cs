using System.Diagnostics;

namespace Ramsha.Authorization;

public interface IPermissionDefinitionManager
{
    public Task<PermissionDefinition?> GetPermissionAsync(string permissionName);
    public Task<IReadOnlyList<PermissionDefinition>> GetPermissionsAsync();

    public Task<PermissionGroupDefinition?> GetGroupAsync(string groupName);
    public Task<IReadOnlyList<PermissionGroupDefinition>> GetGroupsAsync();

}
public class PermissionDefinitionManager(IPermissionDefinitionStore memoryDefinitions) : IPermissionDefinitionManager
{
    public async Task<PermissionDefinition?> GetPermissionAsync(string permissionName)
    {
        return await memoryDefinitions.GetPermissionAsync(permissionName);
    }

    public async Task<PermissionGroupDefinition?> GetGroupAsync(string groupName)
    {
        return await memoryDefinitions.GetGroupAsync(groupName);
    }

    public async Task<IReadOnlyList<PermissionGroupDefinition>> GetGroupsAsync()
    {
        return await memoryDefinitions.GetGroupsAsync();
    }

    public async Task<IReadOnlyList<PermissionDefinition>> GetPermissionsAsync()
    {
        return await memoryDefinitions.GetPermissionsAsync();
    }
}


