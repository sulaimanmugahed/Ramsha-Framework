namespace Ramsha.Authorization;

public class PermissionDefinitionContext : IPermissionDefinitionContext
{
    private readonly Dictionary<string, PermissionGroupDefinition> _groups;
    public Dictionary<string, PermissionGroupDefinition> Groups => _groups;

    public PermissionDefinitionContext()
    {
        _groups = new();
    }

    public void Group(string name, Action<PermissionGroupBuilder> configure)
    {
        var group = new PermissionGroupDefinition { Name = name };

        var groupBuilder = new PermissionGroupBuilder(group);
        configure(groupBuilder);

        _groups.Add(name, group);
    }
}


