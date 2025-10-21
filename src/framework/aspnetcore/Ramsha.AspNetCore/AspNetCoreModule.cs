using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ramsha;

namespace Ramsha.AspNetCore;

public class AspNetCoreModule : RamshaModule
{
    public override void OnModuleCreating(ModuleBuilder context)
    {
        base.OnModuleCreating(context);

    }
    public override void OnAppConfiguring(ConfigureContext context)
    {
        base.OnAppConfiguring(context);
        context.Services.AddObjectAccessor<IApplicationBuilder>();
    }
}






