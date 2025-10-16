
using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.DependencyInjection;

namespace Ramsha;

public interface IRamshaAppWithInternalServiceProvider : IRamshaApp
{
    IServiceProvider CreateServiceProvider();

    Task InitAsync();

    void Init();
}

internal class RamshaAppWithInternalServiceProvider : RamshaAppBase, IRamshaAppWithInternalServiceProvider
{
    public IServiceScope? ServiceScope { get; private set; }

    public RamshaAppWithInternalServiceProvider(
        [NotNull] Type startupModuleType,
        Action<AppCreationOptions>? optionsAction
        ) : this(
        startupModuleType,
        new ServiceCollection(),
        optionsAction)
    {

    }

    private RamshaAppWithInternalServiceProvider(
        [NotNull] Type startupModuleType,
        [NotNull] IServiceCollection services,
        Action<AppCreationOptions>? optionsAction
        ) : base(
            startupModuleType,
            services,
            optionsAction)
    {
        Services.AddSingleton<IRamshaAppWithInternalServiceProvider>(this);
    }

    public IServiceProvider CreateServiceProvider()
    {
        if (ServiceProvider != null)
        {
            return ServiceProvider;
        }

        ServiceScope = Services.BuildServiceProviderFromFactory().CreateScope();
        SetServiceProvider(ServiceScope.ServiceProvider);

        return ServiceProvider!;
    }

    public async Task InitAsync()
    {
        CreateServiceProvider();
        await InitializeModulesAsync();
    }

    public void Init()
    {
        CreateServiceProvider();
        InitializeModules();
    }

    public override void Dispose()
    {
        base.Dispose();
        ServiceScope?.Dispose();
    }
}
