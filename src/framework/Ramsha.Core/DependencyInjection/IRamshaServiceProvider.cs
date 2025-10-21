using Microsoft.Extensions.DependencyInjection;


namespace Ramsha;

public interface IRamshaServiceProvider : IKeyedServiceProvider, IServiceProviderIsService, IServiceProviderIsKeyedService, ISupportRequiredService, IServiceScopeFactory
{
}
