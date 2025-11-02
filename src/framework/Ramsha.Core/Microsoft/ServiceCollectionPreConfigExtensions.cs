
using Microsoft.Extensions.DependencyInjection;
using Ramsha;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionPreConfigExtensions
{
    public static IServiceCollection PreConfigure<TOptions>(this IServiceCollection services, Action<TOptions> optionsAction)
    {
        services.GetPreConfigure<TOptions>().Add(optionsAction);
        return services;
    }

    public static TOptions ExecutePreConfigured<TOptions>(this IServiceCollection services)
        where TOptions : new()
    {
        return services.ExecutePreConfigured(new TOptions());
    }

    public static TOptions ExecutePreConfigured<TOptions>(this IServiceCollection services, TOptions options)
    {
        services.GetPreConfigure<TOptions>().Configure(options);
        return options;
    }

    public static PreConfigureActionList<TOptions> GetPreConfigure<TOptions>(this IServiceCollection services)
    {
        var actionList = services.GetSingletonInstanceOrNull<IObjectAccessor<PreConfigureActionList<TOptions>>>()?.Value;
        if (actionList == null)
        {
            actionList = new PreConfigureActionList<TOptions>();
            services.AddObjectAccessor(actionList);
        }

        return actionList;
    }
}
