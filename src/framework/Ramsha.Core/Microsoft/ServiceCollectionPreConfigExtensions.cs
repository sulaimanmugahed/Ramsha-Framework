
using Microsoft.Extensions.DependencyInjection;
using Ramsha;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionPreConfigExtensions
{
    public static IServiceCollection PrepareConfigure<TOptions>(this IServiceCollection services, Action<TOptions> optionsAction)
    {
        services.GetPrepareConfigureActions<TOptions>().Add(optionsAction);
        return services;
    }

    public static TOptions ExecutePreparedOptions<TOptions>(this IServiceCollection services)
        where TOptions : new()
    {
        return services.ExecutePreparedOptions(new TOptions());
    }

    public static TOptions ExecutePreparedOptions<TOptions>(this IServiceCollection services, TOptions options)
    {
        services.GetPrepareConfigureActions<TOptions>().Configure(options);
        return options;
    }

    public static PreConfigureActionList<TOptions> GetPrepareConfigureActions<TOptions>(this IServiceCollection services)
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
