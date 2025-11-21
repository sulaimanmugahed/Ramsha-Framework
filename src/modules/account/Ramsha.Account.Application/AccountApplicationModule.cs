using Microsoft.Extensions.DependencyInjection;
using Ramsha.Account.Contracts;
using Ramsha.Identity.Domain;

namespace Ramsha.Account.Application;

public class AccountApplicationModule : RamshaModule
{
    public override void OnCreating(ModuleBuilder moduleBuilder)
    {
        base.OnCreating(moduleBuilder);

        moduleBuilder
        .DependsOn<AccountContractsModule>()
        .DependsOn<IdentityDomainModule>();
    }

    public override void OnConfiguring(ConfigureContext context)
    {
        base.OnConfiguring(context);
        context.Services.AddAccountApplicationServices();
    }
}
