using Microsoft.AspNetCore.Mvc;
using Ramsha.Domain;
using Ramsha.LocalMessaging.Abstractions;
using Ramsha.UnitOfWork;
using Ramsha.UnitOfWork.Abstractions;

namespace Ramsha.AspNetCore.Mvc;


public abstract class RamshaControllerBase : ControllerBase
{
    protected IRamshaMediator Mediator => HttpContext.RequestServices.GetLazyRequiredService<IRamshaMediator>().Value;
    protected IUnitOfWorkManager UnitOfWorkManager => HttpContext.RequestServices.GetLazyRequiredService<IUnitOfWorkManager>().Value;
    protected IGlobalQueryFilterManager GlobalQueryFilterManager => HttpContext.RequestServices.GetLazyRequiredService<IGlobalQueryFilterManager>().Value;

}
