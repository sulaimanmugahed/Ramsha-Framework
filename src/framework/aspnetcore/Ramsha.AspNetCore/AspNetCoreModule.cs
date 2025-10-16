using Microsoft.AspNetCore.Builder;

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
