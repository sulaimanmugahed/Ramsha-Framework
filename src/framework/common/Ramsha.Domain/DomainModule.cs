using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ramsha.Security;
using Ramsha.UnitOfWork.Abstractions;

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

        var config = context.Configuration;
        context.Services.Configure<ConnectionStringsOptions>(options =>
        {
            config.GetSection("ConnectionStrings").Bind(options.ConnectionStrings);
            options.Initialize();
        });
        context.Services.Configure<GlobalQueryFilterOptions>(config);

        context.Services.AddDomainServices();




    }
}
public interface IPrice
{
    decimal Price { get; }
}