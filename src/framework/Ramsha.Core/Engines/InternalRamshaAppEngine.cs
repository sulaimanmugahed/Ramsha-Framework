using Microsoft.Extensions.DependencyInjection;

namespace Ramsha;


public interface IInternalRamshaAppEngine : IRamshaAppEngine, IInternalRamshaEngine
{

}

public class InternalRamshaAppEngine : RamshaAppEngine, IInternalRamshaAppEngine
{
    public IServiceScope? ServiceScope { get; private set; }

    public InternalRamshaAppEngine(
        Type startupModuleType,
        Action<AppCreationOptions>? optionsAction = null
        ) : this(
        startupModuleType,
        new ServiceCollection(),
        optionsAction)
    {

    }

    private InternalRamshaAppEngine(
        Type startupModuleType,
         IServiceCollection services,
        Action<AppCreationOptions>? optionsAction = null
        ) : base(
            startupModuleType,
            services,
            optionsAction)
    {
        Services.AddSingleton<IInternalRamshaEngine>(this);
    }

    public IServiceProvider CreateServiceProvider()
    {
        if (ServiceProvider != null)
        {
            return ServiceProvider;
        }

        ServiceScope = Services.BuildServiceProviderFromFactory().CreateScope();
        InternalSetServiceProvider(ServiceScope.ServiceProvider);

        return ServiceProvider!;
    }

    public async Task Initialize()
    {
        CreateServiceProvider();
        await InitializeAsync();
    }


    public override void Dispose()
    {
        base.Dispose();
        ServiceScope?.Dispose();
    }
}
