using Microsoft.Extensions.DependencyInjection;

namespace Ramsha;

public interface IExternalRamshaAppEngine : IRamshaAppEngine, IExternalRamshaEngine
{

}

public class ExternalRamshaAppEngine : RamshaAppEngine, IExternalRamshaAppEngine
{
    public ExternalRamshaAppEngine(
       Type startupModuleType,
        IServiceCollection services,
        Action<AppCreationOptions>? optionsAction = null

        ) : base(
            startupModuleType,
            services,
            optionsAction)
    {
        services.AddSingleton<IExternalRamshaEngine>(this);
    }



    public async Task Initialize(IServiceProvider serviceProvider)
    {
        InternalSetServiceProvider(serviceProvider);
        await InitializeAsync();
    }

    public override void Dispose()
    {
        base.Dispose();

        if (ServiceProvider is IDisposable disposableServiceProvider)
        {
            disposableServiceProvider.Dispose();
        }
    }

    public void SetServiceProvider(IServiceProvider serviceProvider)
    {
        if (ServiceProvider != null)
        {
            if (ServiceProvider != serviceProvider)
            {
                throw new Exception("Service provider was already set before to another service provider instance.");
            }

            return;
        }

        InternalSetServiceProvider(serviceProvider);
    }
}
