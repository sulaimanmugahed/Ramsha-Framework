// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.Controllers;
// using Microsoft.Extensions.DependencyInjection;

// namespace Ramsha.AspNetCore.Mvc;

// public class RamshaControllerActivator : IControllerActivator
// {
//     public object Create(ControllerContext context)
//     {
//         var controllerType = context.ActionDescriptor.ControllerTypeInfo.AsType();
//         var provider = context.HttpContext.RequestServices;
//         var controller = ActivatorUtilities.CreateInstance(provider, controllerType);
//         if (controller is RamshaControllerBase ramshaController)
//         {
//             ramshaController.ServiceProvider = provider.GetRequiredService<IRamshaServiceProvider>();
//         }
//         return controller;

//     }

//     public void Release(ControllerContext context, object controller)
//     {

//     }
// }
