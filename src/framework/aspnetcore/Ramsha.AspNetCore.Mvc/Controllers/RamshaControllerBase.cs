using Microsoft.AspNetCore.Mvc;
using Ramsha.LocalMessaging.Abstractions;

namespace Ramsha.AspNetCore.Mvc;


public abstract class RamshaControllerBase : ControllerBase
{
    protected IRamshaMediator Mediator => HttpContext.RequestServices.GetLazyRequiredService<IRamshaMediator>().Value;
}
