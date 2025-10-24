using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ramsha.Autofac.Extensions;

namespace Ramsha.Autofac;

public class AutofacModule : RamshaModule
{

    public override void OnConfiguring(ConfigureContext context)
    {
        base.OnConfiguring(context);
        //         context.Services.AddAutofac(builder =>
        // {
        //     builder.ConfigureAutofacContainer();
        // });
    }

    public override void OnInit(InitContext context)
    {
        var env = context.ServiceProvider.GetRequiredService<IRamshaHostEnvironment>();
        var logger = context.ServiceProvider.GetService<ILogger<AutofacModule>>();
        logger?.LogInformation("Autofac enabled in environment {env}", env.EnvironmentName);
    }
}
