using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Ramsha.AspNetCore.Mvc;

public class GenericControllerFeatureProvider(TypeInfo[] controllerTypes) : IApplicationFeatureProvider<ControllerFeature>
{
    public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
    {
        foreach (var controllerType in controllerTypes)
        {
            feature.Controllers.Add(controllerType);
        }
    }
}
