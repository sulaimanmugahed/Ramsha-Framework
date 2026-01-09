
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Ramsha.AspNetCore.Mvc;

public class ControllerNameAttribute(string name) : Attribute, IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        controller.ControllerName = name;
        controller.RouteValues["Controller"] = name;
    }
}