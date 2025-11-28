using System.Collections.Concurrent;
using System.Security.Claims;


namespace Ramsha.Authorization;

public interface IPermissionStore
{
    Task<bool> HasPermissionAsync(
        string permissionName,
         string sourceKey);

    Task GrantPermissionAsync(string permissionName, string sourceKey, bool isGrant);
}

public class InMemoryUserPermissionStore : IPermissionStore
{
    private readonly Dictionary<string, HashSet<string>> _userPermissions =
        new(StringComparer.OrdinalIgnoreCase);

    public async Task GrantPermissionAsync(string permissionName, string sourceKey, bool isGrant)
    {
        if (!_userPermissions.ContainsKey(sourceKey))
            _userPermissions[sourceKey] = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        if (isGrant)
        {
            _userPermissions[sourceKey].Add(permissionName);
        }
        else
        {
            _userPermissions[sourceKey].Remove(permissionName);
        }

    }

    public Task<bool> HasPermissionAsync(
        string permissionName,
         string sourceKey)
    {
        if (_userPermissions.TryGetValue(sourceKey, out var perms))
        {
            return Task.FromResult(perms.Contains(permissionName));
        }

        return Task.FromResult(false);
    }
}

public interface IPermissionChecker
{
    Task<bool> HasPermissionAsync(ClaimsPrincipal? claimsPrincipal, string permissionName);
}

public class PermissionChecker(
        IPermissionStore userStore,
        IPermissionDefinitionStore definitionStore) : IPermissionChecker
{
    public async Task<bool> HasPermissionAsync(ClaimsPrincipal? claimsPrincipal, string permissionName)
    {
        var userId = claimsPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return false;


        var allPermissions = await definitionStore.GetAllPermissionsAsync();
        if (!PermissionExists(allPermissions, permissionName))
            return false;


        return await userStore.HasPermissionAsync(permissionName, userId);
    }

    private bool PermissionExists(IEnumerable<PermissionDefinition> permissions, string name)
    {
        foreach (var p in permissions)
        {
            if (p.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                return true;

            if (PermissionExists(p.Children, name))
                return true;
        }
        return false;
    }
}

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

public class PermissionDefinitionManager
{
    private readonly IEnumerable<IPermissionDefinitionProvider> _providers;
    private readonly IPermissionDefinitionContext _context;

    public PermissionDefinitionManager(
        IEnumerable<IPermissionDefinitionProvider> providers,
        IPermissionDefinitionContext context)
    {
        _providers = providers;
        _context = context;
    }

    public void Initialize()
    {
        foreach (var provider in _providers)
            provider.Define(_context);
    }
}


public interface IPermissionDefinitionProvider
{
    void Define(IPermissionDefinitionContext context);
}


public interface IPermissionDefinitionContext
{
    void Group(string name, Action<GroupBuilder> configure);
}

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
}

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

public interface IPermissionDefinitionStore
{
    Task AddGroupAsync(PermissionGroupDefinition group);
    Task AddPermissionAsync(string groupName, PermissionDefinition permission);

    Task<IReadOnlyList<PermissionGroupDefinition>> GetGroupsAsync();
    Task<PermissionGroupDefinition?> GetGroupAsync(string groupName);

    Task<IReadOnlyList<PermissionDefinition>> GetAllPermissionsAsync();
    Task<PermissionDefinition?> GetAsync(string permissionName);

}



public class PermissionGroupDefinition
{
    public string Name { get; set; } = default!;
    public List<PermissionDefinition> Permissions { get; set; } = new();
}
public class PermissionDefinition
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public List<PermissionDefinition> Children { get; set; } = new();
}


