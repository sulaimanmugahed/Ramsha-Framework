using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Ramsha;

namespace Ramsha.AspNetCore
{

}

namespace Microsoft.Extensions.DependencyInjection
{
    public static class WebApplicationBuilderExtensions
    {
        public static async Task<IRamshaAppWithExternalServiceProvider> AddAppAsync<TStartupModule>(
             [NotNull] this WebApplicationBuilder builder,
             Action<AppCreationOptions>? optionsAction = null)
             where TStartupModule : IRamshaModule
        {
            return await builder.Services.AddApplicationAsync<TStartupModule>(options =>
            {
                options.Services.ReplaceConfiguration(builder.Configuration);
                optionsAction?.Invoke(options);
                if (string.IsNullOrWhiteSpace(options.Environment))
                {
                    options.Environment = builder.Environment.EnvironmentName;
                }
            });
        }

        public static async Task<IRamshaAppWithExternalServiceProvider> AddAppAsync(
            [NotNull] this WebApplicationBuilder builder,
            [NotNull] Type startupModuleType,
            Action<AppCreationOptions>? optionsAction = null)
        {
            return await builder.Services.AddApplicationAsync(startupModuleType, options =>
            {
                options.Services.ReplaceConfiguration(builder.Configuration);
                optionsAction?.Invoke(options);
                if (string.IsNullOrWhiteSpace(options.Environment))
                {
                    options.Environment = builder.Environment.EnvironmentName;
                }
            });
        }
    }
}