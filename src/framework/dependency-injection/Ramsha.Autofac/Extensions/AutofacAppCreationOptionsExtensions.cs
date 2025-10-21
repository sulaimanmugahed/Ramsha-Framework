using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace Ramsha.Autofac.Extensions;

public static class AutofacAppCreationOptionsExtensions
{

    public static void UseAutofac(this AppCreationOptions options)
    {
        options.Services.AddAutofacServiceProviderFactory();
    }

    public static RamshaAutofacServiceProviderFactory AddAutofacServiceProviderFactory(this IServiceCollection services)
    {
        return services.AddAutofacServiceProviderFactory(new ContainerBuilder());
    }

    public static RamshaAutofacServiceProviderFactory AddAutofacServiceProviderFactory(this IServiceCollection services, ContainerBuilder containerBuilder)
    {
        var factory = new RamshaAutofacServiceProviderFactory(containerBuilder);

        services.AddObjectAccessor(containerBuilder);
        services.AddSingleton((IServiceProviderFactory<ContainerBuilder>)factory);

        return factory;
    }

}
