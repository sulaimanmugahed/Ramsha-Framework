using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Ramsha;

namespace Microsoft.Extensions.DependencyInjection;

public static class LoggingExtensions
{
    public static ILogger<T> GetBootstrapLogger<T>(this IServiceCollection services)
    {
        var loggerFactory = services.GetSingletonInstanceOrNull<IBootstrapLoggerFactory>();

        return loggerFactory == null ? NullLogger<T>.Instance : loggerFactory.Create<T>();
    }
}
