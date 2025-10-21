using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.Hosting;
namespace Ramsha.Autofac.Microsoft;

public static class HostExtensions
{
    public static IHostBuilder UseAutofac(this IHostBuilder hostBuilder)
    {
        var containerBuilder = new ContainerBuilder();

        return hostBuilder.ConfigureServices((_, services) =>
            {
                services.AddObjectAccessor(containerBuilder);
            })
            .UseServiceProviderFactory(new RamshaAutofacServiceProviderFactory(containerBuilder));
    }
}
