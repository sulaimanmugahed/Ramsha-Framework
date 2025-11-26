using Microsoft.Extensions.DependencyInjection;
using Ramsha.Common.Domain;
using Ramsha.LocalMessaging;

namespace Ramsha.ApplicationAbstractions;

public class CommonApplicationModule : RamshaModule
{

    public override void Register(RegisterContext context)
    {
        base.Register(context);
        context.DependsOn<CommonDomainModule>()
        .DependsOn<LocalMessagingModule>();
    }


    public override void BuildServices(BuildServicesContext context)
    {
        base.BuildServices(context);
        context.Services.AddCommonApplicationServices();
    }
}
