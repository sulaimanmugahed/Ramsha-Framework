namespace Ramsha.Authorization;

public class PermissionBuilder
{
    private readonly PermissionDefinition _permission;

    public PermissionBuilder(PermissionDefinition permission)
    {
        _permission = permission;
    }

    public PermissionBuilder Description(string description)
    {
        _permission.Description = description;
        return this;
    }

    public PermissionBuilder Children(Action<ChildPermissionBuilder> configure)
    {
        var builder = new ChildPermissionBuilder(_permission);
        configure(builder);
        return this;
    }

    public PermissionBuilder WithProviders(string[] providers)
    {
        foreach (var provider in providers)
        {
            _permission.Providers.Add(provider);
        }
        return this;
    }

}


