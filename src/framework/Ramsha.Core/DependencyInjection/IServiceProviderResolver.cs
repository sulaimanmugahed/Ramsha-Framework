using Microsoft.Extensions.DependencyInjection;


namespace Ramsha;

public interface IServiceProviderResolver
{
    object? Resolve(IKeyedServiceProvider serviceProvider, Type serviceType, object? serviceKey);
}
