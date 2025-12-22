namespace Ramsha.Authorization;

public class PermissionDefinition
{
    public string Name { get; set; } = default!;
    public string GroupName { get; set; } = default!;
    public string? Description { get; set; }
    public List<PermissionDefinition> Children { get; set; } = [];
    public List<string> Providers { get; set; } = [];
}


