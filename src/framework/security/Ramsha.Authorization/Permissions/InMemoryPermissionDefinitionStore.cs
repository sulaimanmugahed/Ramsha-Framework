using System.Collections.Concurrent;


namespace Ramsha.Authorization;

public class InMemoryPermissionDefinitionStore : IPermissionDefinitionStore
{
    private readonly ConcurrentDictionary<string, PermissionGroupDefinition> _groups =
        new(StringComparer.OrdinalIgnoreCase);

    public Task AddGroupAsync(PermissionGroupDefinition group)
    {
        _groups[group.Name] = DeepCloneGroup(group);
        return Task.CompletedTask;
    }

    public Task AddPermissionAsync(string groupName, PermissionDefinition permission)
    {
        if (_groups.TryGetValue(groupName, out var group))
        {
            if (!group.Permissions.Any(p => p.Name.Equals(permission.Name, StringComparison.OrdinalIgnoreCase)))
            {
                group.Permissions.Add(DeepClonePermission(permission));
            }
        }

        return Task.CompletedTask;
    }

    public Task<PermissionGroupDefinition?> GetGroupAsync(string groupName)
    {
        _groups.TryGetValue(groupName, out var group);
        return Task.FromResult(group is null ? null : DeepCloneGroup(group));
    }

    public Task<IReadOnlyList<PermissionGroupDefinition>> GetGroupsAsync()
    {
        var list = _groups.Values
            .Select(DeepCloneGroup)
            .ToList()
            .AsReadOnly();

        return Task.FromResult((IReadOnlyList<PermissionGroupDefinition>)list);
    }

    public Task<IReadOnlyList<PermissionDefinition>> GetAllPermissionsAsync()
    {
        var all = _groups.Values
            .SelectMany(g => g.Permissions)
            .Select(DeepClonePermission)
            .ToList()
            .AsReadOnly();

        return Task.FromResult((IReadOnlyList<PermissionDefinition>)all);
    }



    private PermissionGroupDefinition DeepCloneGroup(PermissionGroupDefinition source)
    {
        return new PermissionGroupDefinition
        {
            Name = source.Name,
            Permissions = source.Permissions.Select(DeepClonePermission).ToList()
        };
    }

    private PermissionDefinition DeepClonePermission(PermissionDefinition source)
    {
        return new PermissionDefinition
        {
            Name = source.Name,
            Description = source.Description,
            Children = source.Children.Select(DeepClonePermission).ToList()
        };
    }

    public Task<PermissionDefinition?> GetAsync(string name)
    {
        foreach (var group in _groups.Values)
        {
            var found = FindPermission(group.Permissions, name);
            if (found != null)
                return Task.FromResult<PermissionDefinition?>(DeepClonePermission(found));
        }

        return Task.FromResult<PermissionDefinition?>(null);
    }

    private PermissionDefinition? FindPermission(IEnumerable<PermissionDefinition> list, string name)
    {
        foreach (var p in list)
        {
            if (p.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                return p;

            var child = FindPermission(p.Children, name);
            if (child != null)
                return child;
        }

        return null;
    }
}


