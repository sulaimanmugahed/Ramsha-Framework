using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Ramsha;

namespace Ramsha.AspNetCore;

public class AspNetCoreModule : RamshaModule
{
    public override void OnCreating(ModuleBuilder moduleBuilder)
    {
        base.OnCreating(moduleBuilder);
    }
    public override void OnConfiguring(ConfigureContext context)
    {
        base.OnConfiguring(context);
        context.Services.TryAddSingleton<IAppPipeline<IApplicationBuilder>, AppPipeline<IApplicationBuilder>>();
    }

    public override void OnInit(InitContext context)
    {
        base.OnInit(context);

        var aspNetCoreOptions = context.GetOptions<AspNetCoreOptions>().Value;
        var appPipeline = context.GetAppPipelineBuilder();

        var entryOptions = new PipelineEntryOptions
        {
            CanMove = false,
            CanRemove = false
        };

        appPipeline.Use(AspNetCorePipelineEntries.HttpsRedirection, app =>
        {
            app.UseHttpsRedirection();
        }, entryOptions, () => aspNetCoreOptions.HttpsRedirection);

        appPipeline.Use(AspNetCorePipelineEntries.StaticFiles, app =>
        {
            app.UseStaticFiles();
        }, entryOptions);

        appPipeline.Use(AspNetCorePipelineEntries.UnitOfWork, app =>
        {
            app.UseUnitOfWork();
        }, entryOptions);

        appPipeline.Use(AspNetCorePipelineEntries.Routing, app =>
        {
            app.UseRouting();
        }, entryOptions);

        appPipeline.Use(AspNetCorePipelineEntries.Authentication, app =>
        {
            app.UseAuthentication();
        }, entryOptions);

        appPipeline.Use(AspNetCorePipelineEntries.Authorization, app =>
        {
            app.UseAuthorization();
        }, entryOptions);

        appPipeline.Use(AspNetCorePipelineEntries.Endpoints, app =>
        {
            app.UseRamshaEndpoints();
        }, entryOptions);
    }
}






