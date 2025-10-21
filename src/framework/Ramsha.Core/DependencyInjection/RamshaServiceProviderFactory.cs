using Microsoft.Extensions.DependencyInjection;


namespace Ramsha;

public class RamshaServiceProviderFactory : IServiceProviderFactory<IServiceCollection>
{
    public static readonly ServiceProviderOptions DefaultServiceProviderOptions = new() { ValidateOnBuild = true, ValidateScopes = true };

    private IServiceProvider? _serviceProvider;
    private readonly IServiceProviderResolver? _resolver;
    private readonly ServiceProviderOptions? _options;

    public RamshaServiceProviderFactory(ServiceProviderOptions? options = null, IServiceProviderResolver? resolver = null)
    {
        _options = options;
        _resolver = resolver;
    }

    public IServiceCollection CreateBuilder(IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        return services;
    }

    public IServiceProvider CreateServiceProvider(IServiceCollection containerBuilder)
    {
        ArgumentNullException.ThrowIfNull(containerBuilder, nameof(containerBuilder));
        return _serviceProvider is null ? _serviceProvider = new RamshaServiceProvider(containerBuilder, _options, _resolver) : _serviceProvider;
    }
}
