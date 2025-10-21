using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Ramsha.Autofac.Extensions;

namespace Ramsha.Autofac.Microsoft;

public static class AutofacServiceCollectionExtensions
{
    public static ContainerBuilder GetContainerBuilder([NotNull] this IServiceCollection services)
    {

        var builder = services.GetObjectOrNull<ContainerBuilder>();
        if (builder == null)
        {
            throw new Exception($"Could not find ContainerBuilder. Be sure that you have called {nameof(AutofacAppCreationOptionsExtensions.UseAutofac)} method before!");
        }

        return builder;
    }

    public static IServiceProvider BuildAutofacServiceProvider([NotNull] this IServiceCollection services, Action<ContainerBuilder>? builderAction = null)
    {
        return services.BuildServiceProviderFromFactory(builderAction);
    }
}
