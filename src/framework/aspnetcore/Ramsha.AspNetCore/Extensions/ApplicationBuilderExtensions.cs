using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Ramsha;

namespace Ramsha.AspNetCore
{

}

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public async static Task UseRamshaAsync([NotNull] this IApplicationBuilder app)
        {

            var application = app.ApplicationServices.GetRequiredService<IRamshaAppWithExternalServiceProvider>();
            var applicationLifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

            applicationLifetime.ApplicationStopping.Register(() =>
            {
                AsyncHelper.RunSync(() => application.ShutdownAsync());
            });

            applicationLifetime.ApplicationStopped.Register(() =>
            {
                application.Dispose();
            });

            await application.InitAsync(app.ApplicationServices);

            var pipeline = app.ApplicationServices.GetRequiredService<IAppPipeline<IApplicationBuilder>>();
            pipeline.Apply(app);

        }

        public static void UseRamsha([NotNull] this IApplicationBuilder app)
        {

            app.ApplicationServices.GetRequiredService<ObjectAccessor<IApplicationBuilder>>().Value = app;
            var application = app.ApplicationServices.GetRequiredService<IRamshaAppWithExternalServiceProvider>();
            var applicationLifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

            applicationLifetime.ApplicationStopping.Register(() =>
            {
                application.Shutdown();
            });

            applicationLifetime.ApplicationStopped.Register(() =>
            {
                application.Dispose();
            });

            application.Init(app.ApplicationServices);

            var pipeline = app.ApplicationServices.GetRequiredService<IAppPipeline<IApplicationBuilder>>();
            pipeline.Apply(app);
        }

         public static IApplicationBuilder UseRamshaEndpoints(
        this IApplicationBuilder app,
        Action<IEndpointRouteBuilder>? additionalConfigurationAction = null)
    {
        var options = app.ApplicationServices
            .GetRequiredService<IOptions<RamshaEndpointRouterOptions>>()
            .Value;

        if (!options.EndpointConfigureActions.Any() && additionalConfigurationAction == null)
        {
            return app;
        }

        return app.UseEndpoints(endpoints =>
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                if (options.EndpointConfigureActions.Any())
                {
                    var context = new EndpointRouteBuilderContext(endpoints, scope.ServiceProvider);

                    foreach (var configureAction in options.EndpointConfigureActions)
                    {
                        configureAction(context);
                    }
                }
            }
            
            additionalConfigurationAction?.Invoke(endpoints);
        });
    }
    }
}