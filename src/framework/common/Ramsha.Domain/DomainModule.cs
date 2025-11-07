using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ramsha.Security;

namespace Ramsha.Domain;

public class DomainModule : RamshaModule
{

    public override void OnCreating(ModuleBuilder moduleBuilder)
    {
        base.OnCreating(moduleBuilder);
        moduleBuilder.DependsOn<SecurityModule>();
    }

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
        context.Services.Configure<GlobalQueryFilterOptions>(config);
        context.Services.AddSingleton<IGlobalQueryFilterManager, GlobalQueryFilterManager>();

        context.Services.AddSingleton(typeof(IGlobalQueryFilterManager<>), typeof(GlobalQueryFilterManager<>));



    }
}
public interface IPrice
{
    decimal Price { get; }
}