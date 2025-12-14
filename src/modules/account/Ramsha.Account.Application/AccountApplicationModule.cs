using Microsoft.Extensions.DependencyInjection;
using Ramsha.Account.Contracts;
using Ramsha.Common.Application;
using Ramsha.Identity.Domain;

namespace Ramsha.Account.Application;

public class AccountApplicationModule : RamshaModule
{
    public override void Register(RegisterContext context)
    {
        base.Register(context);
        context
        .DependsOn<AccountContractsModule>()
        .DependsOn<IdentityDomainModule>()
        .DependsOn<CommonApplicationModule>();
    }


    public override void BuildServices(BuildServicesContext context)
    {
        base.BuildServices(context);
        context.Services.AddAccountApplicationServices();
    }
}
