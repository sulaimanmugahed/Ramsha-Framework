namespace Ramsha.Authorization;

public class ChildPermissionBuilder
{
    private readonly PermissionDefinition _parent;

    public ChildPermissionBuilder(PermissionDefinition parent)
    {
        _parent = parent;
    }

    public PermissionDefinition Add(string name, string? description = null)
    {
        var child = new PermissionDefinition
        {
            Name = name,
            Description = description
        };

        _parent.Children.Add(child);
        return child;
    }
}


