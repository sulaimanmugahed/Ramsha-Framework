using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Ramsha.AspNetCore;

public static class HostBuilderExtensions
{
    public static IHostBuilder UseRamshaServiceProvider(this IHostBuilder builder, ServiceProviderOptions? options = null, IServiceProviderResolver? resolver = null)
    {
        ArgumentNullException.ThrowIfNull(builder, nameof(builder));
        var serviceProviderFactory = new RamshaServiceProviderFactory(options ?? RamshaServiceProviderFactory.DefaultServiceProviderOptions, resolver);
        return builder
            .UseServiceProviderFactory(serviceProviderFactory);

    }
}
