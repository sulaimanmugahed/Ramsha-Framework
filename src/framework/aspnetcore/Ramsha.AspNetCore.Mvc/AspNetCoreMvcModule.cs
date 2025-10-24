﻿
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;

namespace Ramsha.AspNetCore.Mvc;

public class AspNetCoreMvcModule : RamshaModule
{
    public override void OnCreating(ModuleBuilder moduleBuilder)
    {
        base.OnCreating(moduleBuilder);
        moduleBuilder.DependsOn<AspNetCoreModule>();
    }


    public override void OnConfiguring(ConfigureContext context)
    {
        base.OnConfiguring(context);

        context.Services.AddMvc();
        context.Services.AddOpenApi();

        context.Services.AddSingleton<IOptionsSnapshot<ScalarOptions>>(sp =>
       {
           ScalarOptions options = new();
           return new StaticOptions<ScalarOptions>(options);
       });

        context.Services.Configure<RamshaEndpointRouterOptions>(options =>
         {
             options.EndpointConfigureActions.Add(endpointContext =>
             {
                 endpointContext.Endpoints.MapControllerRoute("defaultWithArea", "{area}/{controller=Home}/{action=Index}/{id?}");
                 endpointContext.Endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}").WithStaticAssets();
                 endpointContext.Endpoints.MapRazorPages().WithStaticAssets();
                 endpointContext.Endpoints.MapOpenApi("/openapi/{documentName}.json");
                 endpointContext.Endpoints.MapScalarApiReference(options =>
                 {
                     options.Title = "Weather Forecast API Sample";
                     options.Theme = ScalarTheme.Default;
                     options.Favicon = "/favicon.svg";
                     options.Layout = ScalarLayout.Modern;
                     options.DarkMode = true;
                     options.CustomCss = "* { font-family: 'Monaco'; }";
                     options.OpenApiRoutePattern = "/openapi/{documentName}.json";
                 });
             });
         });


    }

    public override void OnInit(InitContext context)
    {
        base.OnInit(context);

        var env = context.GetEnvironment();

        context.GetAppPipelineBuilder()
        .Use(AspNetCoreMvcPipelineEntries.Endpoints, app =>
        {
            app.UseRouting();
            app.UseRamshaEndpoints();
        });
    }

}
