using Microsoft.Extensions.DependencyInjection;
using Ramsha.AspNetCore.Mvc;
using Ramsha.Identity.Contracts;
using Ramsha.Identity.Core.Options;

namespace Ramsha.Identity.Api;

public class IdentityApiModule : RamshaModule
{
    public override void OnCreating(ModuleBuilder moduleBuilder)
    {
        base.OnCreating(moduleBuilder);
        moduleBuilder.DependsOn<IdentityContractsModule>();

        moduleBuilder.OnCreatingConfigure<IMvcBuilder>(builder =>
        {
            builder.AddIdentityGenericControllers();
        });
    }
}
