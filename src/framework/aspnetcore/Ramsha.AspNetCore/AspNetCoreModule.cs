using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Ramsha.AspNetCore.Security.Claims;
using Ramsha.Common.Domain;
using Ramsha.Security.Claims;
using Ramsha.UnitOfWork.Abstractions;

namespace Ramsha.AspNetCore;

public class AspNetCoreModule : RamshaModule
{
    public override void Register(RegisterContext context)
    {
        base.Register(context);

        context.DependsOn<CommonDomainModule>();

    }

    public override void Prepare(PrepareContext context)
    {
        base.Prepare(context);

    }
    public override void BuildServices(BuildServicesContext context)
    {
        base.BuildServices(context);
        context.Services.AddHttpContextAccessor();
        context.Services.AddSingleton<IPrincipalAccessor, HttpPrincipalAccessor>();
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

        var unitOfWorkOptions = context.ServiceProvider.GetRequiredService<IOptions<GlobalUnitOfWorkOptions>>().Value;

        appPipeline.Use(AspNetCorePipelineEntries.UnitOfWork, app =>
        {
            app.UseUnitOfWork();
        }, entryOptions, () => unitOfWorkOptions.IsEnabled);

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






