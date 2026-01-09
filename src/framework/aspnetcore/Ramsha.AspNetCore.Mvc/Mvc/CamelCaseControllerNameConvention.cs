using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Ramsha.AspNetCore.Mvc;

public class CamelCaseControllerNameAttribute : Attribute,
IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        controller.ControllerName = ToCamelCase(controller.ControllerName);
    }

    private static string ToCamelCase(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        return char.ToLowerInvariant(input[0]) + input.Substring(1);
    }

}

