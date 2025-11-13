using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace Ramsha.AspNetCore.Mvc;

public class GenericApplicationPart : ApplicationPart, IApplicationPartTypeProvider
{
    public GenericApplicationPart(Type type)
    {
        Types = [type.GetTypeInfo()];
    }

    public override string Name => "GenericApplicationPart";

    public IEnumerable<TypeInfo> Types { get; }
}
