using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha.Common.Domain;

public class ConnectionStringsOptions
{
    private readonly Dictionary<string, string> _lookup = new(StringComparer.OrdinalIgnoreCase);
    public Dictionary<string, string> ConnectionStrings { get; } = new(StringComparer.OrdinalIgnoreCase);

    public string? Default { get; private set; }

    internal void Initialize()
    {
        _lookup.Clear();
        foreach (var kvp in ConnectionStrings)
        {
            _lookup[kvp.Key] = kvp.Value;
        }

        Default = ConnectionStrings.ContainsKey("Default") ? ConnectionStrings["Default"] : null;
    }


    public void ConfigureAliases(Action<AliasMappingBuilder> configureAction)
    {
        var builder = new AliasMappingBuilder(_lookup, ConnectionStrings);
        configureAction(builder);
    }


    public string Get(string nameOrAlias)
    {
        return _lookup.TryGetValue(nameOrAlias, out var conn) ? conn : Default!;
    }


    public void SetConnectionString(string name, string connectionString)
    {
        ConnectionStrings[name] = connectionString;
        _lookup[name] = connectionString;
        if (Default == null) Default = connectionString;
    }



}

public class AliasMappingBuilder
{
    private readonly Dictionary<string, string> _lookup;
    private readonly Dictionary<string, string> _connections;

    internal AliasMappingBuilder(Dictionary<string, string> lookup, Dictionary<string, string> connections)
    {
        _lookup = lookup;
        _connections = connections;
    }

    public void Map(string connectionName, string alias)
    {
        if (!_connections.ContainsKey(connectionName))
            throw new KeyNotFoundException($"Connection '{connectionName}' not found.");

        _lookup[alias] = _connections[connectionName];
    }

    public void Map(string connectionName, string[] aliases)
    {
        _lookup[connectionName] = _connections[connectionName];
        foreach (var alias in aliases)
        {
            Map(connectionName, alias);
        }
    }
}