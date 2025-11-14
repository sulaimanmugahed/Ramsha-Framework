using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Ramsha.Domain;

public static class EntityHelper
{
    public static TId? ConvertId<TId>(string stringId)
    where TId : IEquatable<TId>
    {
        var id = default(TId);

        var converter = TypeDescriptor.GetConverter(typeof(TId));
        if (converter != null && converter.CanConvertFrom(typeof(string)))
        {
            id = (TId)converter.ConvertFromInvariantString(stringId);
        }
        else
        {
            id = (TId)Convert.ChangeType(stringId, typeof(TId));
        }
        return id;
    }
    public static Type? FindPrimaryKeyType(Type entityType)
    {
        if (!typeof(IEntity).IsAssignableFrom(entityType))
        {
            throw new Exception(
                $"Given {nameof(entityType)} is not an entity. It should implement {typeof(IEntity).AssemblyQualifiedName}!");
        }

        foreach (var interfaceType in entityType.GetTypeInfo().GetInterfaces())
        {
            if (interfaceType.GetTypeInfo().IsGenericType &&
                interfaceType.GetGenericTypeDefinition() == typeof(IEntity<>))
            {
                return interfaceType.GenericTypeArguments[0];
            }
        }

        return null;
    }
}
