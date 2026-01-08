
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
        public static void UseRamsha(this IApplicationBuilder app)
        {
            var engine = app.ApplicationServices.GetRequiredService<IExternalRamshaEngine>();
            var applicationLifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

            applicationLifetime.ApplicationStopping.Register(() =>
            {
                AsyncHelper.RunSync(() => engine.ShutdownAsync());
            });

            if (engine is IDisposable disposableEngine)
            {
                applicationLifetime.ApplicationStopped.Register(disposableEngine.Dispose);
            }

            Task.Run(() => engine.Initialize(app.ApplicationServices))
            .GetAwaiter()
            .GetResult();

            var pipeline = app.ApplicationServices.GetService<IAppPipeline<IApplicationBuilder>>();
            if (pipeline is not null)
            {
                pipeline.Apply(app);
            }
        }

        public static async Task UseRamshaAsync(
          this IApplicationBuilder app)
        {
            var engine = app.ApplicationServices.GetRequiredService<IExternalRamshaEngine>();
            var applicationLifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

            applicationLifetime.ApplicationStopping.Register(() =>
            {
                AsyncHelper.RunSync(() => engine.ShutdownAsync());
            });

            if (engine is IDisposable disposableEngine)
            {
                applicationLifetime.ApplicationStopped.Register(disposableEngine.Dispose);
            }

            await engine.Initialize(app.ApplicationServices);

            var pipeline = app.ApplicationServices.GetService<IAppPipeline<IApplicationBuilder>>();
            if (pipeline is not null)
            {
                pipeline.Apply(app);
            }
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