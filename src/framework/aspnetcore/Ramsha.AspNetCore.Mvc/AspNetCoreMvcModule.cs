
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Ramsha.AspNetCore.Mvc;

public class AspNetCoreMvcModule : RamshaModule
{
    public override void OnModuleCreating(ModuleBuilder moduleBuilder)
    {
        base.OnModuleCreating(moduleBuilder);
        moduleBuilder.DependsOn<AspNetCoreModule>();
    }


    public override void OnAppConfiguring(ConfigureContext context)
    {
        context.Services.AddControllers();

      //  context.Services.AddSingleton(typeof(IAppPipeline<IApplicationBuilder>), typeof(AppPipeline<IApplicationBuilder>));

        //context.Services.Replace(ServiceDescriptor.Transient<IControllerActivator, RamshaControllerActivator>());
        base.OnAppConfiguring(context);
    }






}
