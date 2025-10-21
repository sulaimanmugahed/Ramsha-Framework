using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac.Core;

namespace Ramsha.Autofac;

public class RamshaInjectPropertySelector(bool preserveSetValues) : DefaultPropertySelector(preserveSetValues)
{
    // public bool InjectProperty(PropertyInfo propertyInfo, object instance)
    // {
    //     return propertyInfo.CanWrite &&
    //            propertyInfo.GetCustomAttribute<InjectAttribute>(inherit: true) != null;
    // }
}

