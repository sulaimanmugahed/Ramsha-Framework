using System.Collections.Concurrent;
using System.Collections.Immutable;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;


namespace Ramsha.Authorization;

public class InMemoryPermissionDefinitionStore : IPermissionDefinitionStore
{
    private readonly Lazy<Dictionary<string, PermissionGroupDefinition>> _lazyGroupDefinitions;
    private IDictionary<string, PermissionGroupDefinition> GroupDefinitions => _lazyGroupDefinitions.Value;
    private readonly Lazy<Dictionary<string, PermissionDefinition>> _lazyPermissionDefinitions;
    private IDictionary<string, PermissionDefinition> PermissionsDefinitions => _lazyPermissionDefinitions.Value;
    private readonly IServiceProvider _serviceProvider;
    private readonly RamshaPermissionOptions _options;


    public InMemoryPermissionDefinitionStore(
        IServiceProvider serviceProvider
    , IOptions<RamshaPermissionOptions> options)
    {
        _options = options.Value;
        _serviceProvider = serviceProvider;
        _lazyGroupDefinitions = new(CreateGroups, true);
        _lazyPermissionDefinitions = new(CreatePermission, true);
    }

    private Dictionary<string, PermissionGroupDefinition> CreateGroups()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = new PermissionDefinitionContext();

        var providers = _options
        .DefinitionProviders
        .Select(dp => scope.ServiceProvider.GetRequiredService(dp) as IPermissionDefinitionProvider)
        .ToList();

        foreach (var provider in providers)
            provider?.Define(context);

        return context.Groups;
    }


    private Dictionary<string, PermissionDefinition> CreatePermission()
    {
        var permissions = new Dictionary<string, PermissionDefinition>();
        foreach (var group in GroupDefinitions.Values)
        {
            foreach (var permission in group.Permissions)
            {
                AddPermissionRecurve(permission, permissions);
            }
        }

        return permissions;
    }

    private void AddPermissionRecurve(PermissionDefinition permission, Dictionary<string, PermissionDefinition> permissions)
    {
        var name = permission.Name;
        if (permissions.ContainsKey(name))
        {
            throw new Exception("duplicate permission");
        }

        permissions[name] = permission;

        foreach (var child in permission.Children)
        {
            AddPermissionRecurve(child, permissions);
        }
    }

    public Task<PermissionGroupDefinition?> GetGroupAsync(string groupName)
    {
        GroupDefinitions.TryGetValue(groupName, out var group);
        return Task.FromResult(group);
    }

    public Task<IReadOnlyList<PermissionGroupDefinition>> GetGroupsAsync()
    {
        return Task.FromResult<IReadOnlyList<PermissionGroupDefinition>>(GroupDefinitions.Values.ToImmutableArray());
    }

    public Task<IReadOnlyList<PermissionDefinition>> GetPermissionsAsync()
    {
        return Task.FromResult<IReadOnlyList<PermissionDefinition>>(PermissionsDefinitions.Values.ToImmutableArray());
    }

    public Task<PermissionDefinition?> GetPermissionAsync(string name)
    {
        PermissionsDefinitions.TryGetValue(name, out var permission);
        return Task.FromResult(permission);
    }

}


