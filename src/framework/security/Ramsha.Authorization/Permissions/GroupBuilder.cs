namespace Ramsha.Authorization;

public class GroupBuilder
{
    private readonly PermissionGroupDefinition _group;

    public GroupBuilder(PermissionGroupDefinition group)
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


