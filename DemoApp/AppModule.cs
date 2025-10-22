using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoModule;
using LiteBus.Extensions.Microsoft.DependencyInjection;
using LiteBus.Queries;
using Ramsha;
using Ramsha.AspNetCore;
using Ramsha.AspNetCore.Mvc;
using Ramsha.LocalMessaging;

namespace DemoApp;

public class AppModule : RamshaModule
{
    public override void OnModuleCreating(ModuleBuilder moduleBuilder)
    {
        base.OnModuleCreating(moduleBuilder);

        moduleBuilder
        .DependsOn<DemoModuleModule>()
        .DependsOn<AspNetCoreMvcModule>()
        .DependsOn<LocalMessagingModule>();
    }

    public override void OnAppConfiguring(ConfigureContext context)
    {
        base.OnAppConfiguring(context);
        context.Services.AddScoped<IRamshaService, RamshaService>();
        context.Services.AddScoped<ITestService, TestService>();
        context.Services.AddOpenApi();
        context.Services.AddLiteBus(options =>
      {

          options.AddQueryModule(builder =>
          {

              builder.RegisterFromAssembly(typeof(TestQuery).Assembly);
          });

      });


    }

    public override void OnAppInit(InitContext context)
    {
        base.OnAppInit(context);

        var logger = context.ServiceProvider.GetRequiredService<ILogger<Program>>();
        context.GetAppPipelineBuilder()
        .Use("first", app =>
        {
            logger.LogInformation("************ First pipeline...");
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
        }, 1)
        .Use(app =>
        {
            logger.LogInformation("************ Second pipeline...");
            app.UseEndpoints(end =>
             {
                 end.MapGet("/get", () => "pong");
             });
        }, 2)
        .UseAfter("first", app =>
        {
            logger.LogInformation("************ middle pipeline...");
        });
    }

    public override void OnAppShutdown(ShutdownContext context)
    {
        context.ServiceProvider.GetRequiredService<ILogger<AppModule>>().LogError("application shutdown");
        base.OnAppShutdown(context);
    }
}
