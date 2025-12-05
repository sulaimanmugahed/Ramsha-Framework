namespace Ramsha.Authorization;

public class PermissionGroupBuilder
{
    private readonly PermissionGroupDefinition _group;

    public PermissionGroupBuilder(PermissionGroupDefinition group)
    {
        _group = group;
    }

    public PermissionBuilder Permission(string name, string? description = null)
    {
        var permission = new PermissionDefinition
        {
            Name = name,
            Description = description
        };

        _group.Permissions.Add(permission);

        return new PermissionBuilder(permission);
    }
}


