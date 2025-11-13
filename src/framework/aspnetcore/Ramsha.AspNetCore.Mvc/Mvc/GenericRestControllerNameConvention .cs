using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Ramsha.AspNetCore.Mvc;

public class GenericControllerName(string name) : Attribute, IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        if (!controller.ControllerType.IsGenericType)
        {
            return;
        }

        controller.ControllerName = name;
        controller.RouteValues["Controller"] = name;
    }
}