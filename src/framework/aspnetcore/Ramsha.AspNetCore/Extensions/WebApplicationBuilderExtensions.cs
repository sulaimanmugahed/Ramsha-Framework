using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Ramsha;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class WebApplicationBuilderExtensions
    {

        public static IExternalRamshaAppEngine AddRamshaApp<TStartupModule>(
           [NotNull] this WebApplicationBuilder builder,
           Action<AppCreationOptions>? optionsAction = null)
           where TStartupModule : IRamshaModule
        {
            return builder.AddRamshaApp(typeof(TStartupModule), optionsAction);
        }


        public static IExternalRamshaAppEngine AddRamshaApp(
           [NotNull] this WebApplicationBuilder builder,
            [NotNull] Type startupModuleType,
           Action<AppCreationOptions>? optionsAction = null)

        {
            return builder.Services.AddRamshaApp(startupModuleType, options =>
            {
                options.Services.ReplaceConfiguration(builder.Configuration);
                optionsAction?.Invoke(options);
                if (string.IsNullOrWhiteSpace(options.Environment))
                {
                    options.Environment = builder.Environment.EnvironmentName;
                }
            });
        }




        public static async Task<IExternalRamshaAppEngine> AddRamshaAppAsync<TStartupModule>(
             [NotNull] this WebApplicationBuilder builder,
             Action<AppCreationOptions>? optionsAction = null)
             where TStartupModule : IRamshaModule
        {
            return await builder.AddRamshaAppAsync(typeof(TStartupModule), optionsAction);
        }

        public static async Task<IExternalRamshaAppEngine> AddRamshaAppAsync(
            [NotNull] this WebApplicationBuilder builder,
            [NotNull] Type startupModuleType,
            Action<AppCreationOptions>? optionsAction = null)
        {
            return await builder.Services.AddRamshaAppAsync(startupModuleType, options =>
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