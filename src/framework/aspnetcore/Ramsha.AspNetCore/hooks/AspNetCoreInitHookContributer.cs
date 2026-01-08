using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Ramsha.UnitOfWork.Abstractions;

namespace Ramsha.AspNetCore;

public class AspNetCoreInitHookContributor : IInitHookContributor
{
    public Task OnInitialize(InitContext context)
    {
        var aspNetCoreOptions = context.GetOptions<AspNetCoreOptions>().Value;
        var appPipeline = context.GetAppPipelineBuilder();

        var entryOptions = new PipelineEntryOptions
        {
            CanMove = false,
            CanRemove = false
        };

        appPipeline.Use(AspNetCorePipelineEntries.ExceptionHandler, app =>
        {
            app.UseExceptionHandler();
        }, entryOptions, () => aspNetCoreOptions.ExceptionHandler);

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

        return Task.CompletedTask;
    }
}
