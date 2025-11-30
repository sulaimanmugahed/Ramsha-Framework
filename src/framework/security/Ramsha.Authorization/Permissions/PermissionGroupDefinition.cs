namespace Ramsha.Authorization;

public class PermissionGroupDefinition
{
    public string Name { get; set; } = default!;
    public List<PermissionDefinition> Permissions { get; set; } = [];
}


