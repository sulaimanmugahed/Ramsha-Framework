using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha;

public class RamshaTypeHelpers
{

    public static Type[] GetRamshaTypes(Type baseType)
    {
        return GetImplementationTypes<IRamshaModule>(baseType);

    }
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
    public static Type[] GetImplementationTypes<TAssemblyMarker>(Type baseType)
    {
        //     RamshaAssemblyHelpers.LoadAllDlls();
        //     return RamshaAssemblyHelpers.GetAssembliesWithAccessTo(typeof(TAssemblyMarker))
        //          .SelectMany(a => a.GetTypes())
        //    .Where(t => t.IsClass && !t.IsAbstract && baseType.IsAssignableFrom(t))
        //    .ToArray();
        return RamshaAssemblyHelpers.GetAssembliesWithAccessTo(typeof(TAssemblyMarker))
     .SelectMany(a => a.GetTypes())
     .Where(t => t.IsClass && !t.IsAbstract && baseType.IsAssignableFrom(t))
     .ToArray();
    }


}
