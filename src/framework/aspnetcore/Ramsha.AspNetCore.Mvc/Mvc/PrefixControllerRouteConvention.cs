
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Ramsha.AspNetCore.Mvc;

public class ControllerRoutePrefixAttribute(string prefix) : Attribute, IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {

        if (!controller.Selectors.Any())
            return;

        foreach (var selector in controller.Selectors)
        {
            if (selector.AttributeRouteModel != null)
            {
                selector.AttributeRouteModel =
                    AttributeRouteModel.CombineAttributeRouteModel(
                        new AttributeRouteModel
                        {
                            Template = prefix
                        },
                        selector.AttributeRouteModel);
            }
        }
    }
}
