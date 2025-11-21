using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Ramsha.Common.Domain;

public sealed class ConnectionStringAttribute : Attribute
{
    public string Name { get; }

    public ConnectionStringAttribute(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public static string? GetNameOrNull<T>() => GetNameOrNull(typeof(T));

    public static string? GetNameOrNull(Type type)
    {
        return type.GetTypeInfo().GetCustomAttribute<ConnectionStringAttribute>()?.Name;
    }
}
