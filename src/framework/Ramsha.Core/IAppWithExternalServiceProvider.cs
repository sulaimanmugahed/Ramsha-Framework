using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Ramsha;

public interface IRamshaAppWithExternalServiceProvider : IRamshaApp
{
    void SetServiceProvider([NotNull] IServiceProvider serviceProvider);
    Task InitAsync([NotNull] IServiceProvider serviceProvider);
    void Init([NotNull] IServiceProvider serviceProvider);
}
internal class RamshaAppWithExternalServiceProvider : RamshaAppBase, IRamshaAppWithExternalServiceProvider
{
    public RamshaAppWithExternalServiceProvider(
        [NotNull] Type startupModuleType,
        [NotNull] IServiceCollection services,
        Action<AppCreationOptions>? optionsAction,
        IRamshaModule? startupModuleInstance = null


        ) : base(
            startupModuleType,
            services,
            optionsAction,
            startupModuleInstance)
    {
        services.AddSingleton<IRamshaAppWithExternalServiceProvider>(this);
    }

    void IRamshaAppWithExternalServiceProvider.SetServiceProvider([NotNull] IServiceProvider serviceProvider)
    {

        if (ServiceProvider != null)
        {
            if (ServiceProvider != serviceProvider)
            {
                throw new Exception("Service provider was already set before to another service provider instance.");
            }

            return;
        }

        SetServiceProvider(serviceProvider);
    }

    public async Task InitAsync([NotNull] IServiceProvider serviceProvider)
    {
        SetServiceProvider(serviceProvider);
        await InitializeModulesAsync();
    }

    public void Init([NotNull] IServiceProvider serviceProvider)
    {
        SetServiceProvider(serviceProvider);

        InitializeModules();
    }

    public override void Dispose()
    {
        base.Dispose();

        if (ServiceProvider is IDisposable disposableServiceProvider)
        {
            disposableServiceProvider.Dispose();
        }
    }
}