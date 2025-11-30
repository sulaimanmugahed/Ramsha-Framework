namespace Ramsha.Authorization;

public class PermissionDefinitionContext : IPermissionDefinitionContext
{
    private readonly IPermissionDefinitionStore _store;

    public PermissionDefinitionContext(IPermissionDefinitionStore store)
    {
        _store = store;
    }

    public void Group(string name, Action<GroupBuilder> configure)
    {
        var group = new PermissionGroupDefinition { Name = name };

        var groupBuilder = new GroupBuilder(group);
        configure(groupBuilder);

        _store.AddGroupAsync(group);
    }
}


