using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ramsha.Domain;

public class DomainModule : RamshaModule
{


    public override void OnConfiguring(ConfigureContext context)
    {
        base.OnConfiguring(context);
        context.Services.AddTransient<IConnectionStringResolver, DefaultConnectionStringResolver>();


        var config = context.Services.GetConfiguration();
        context.Services.Configure<ConnectionStringsOptions>(options =>
        {
            config.GetSection("ConnectionStrings").Bind(options.ConnectionStrings);
            options.Initialize();
        });

       


    }
}
