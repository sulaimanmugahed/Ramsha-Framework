
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Ramsha;

public class RegisterContext
{
    private readonly IRamshaModuleRegistry _registry;
    private readonly Type _currentModule;
    private readonly IServiceCollection _services;
    private readonly ILogger _logger;
    private readonly IConfiguration _configuration;

    public RegisterContext(
        IRamshaModuleRegistry registry,
        Type currentModule,
        IServiceCollection services,
        ILogger? logger = null,
        IConfiguration? configuration = null)
    {
        _registry = registry;
        _currentModule = currentModule;
        _services = services;
        _logger = logger ?? NullLogger.Instance;
        _configuration = configuration ?? services.GetConfiguration();
    }

    public IConfiguration Configuration => _configuration;
    public ILogger Logger => _logger;



    public RegisterContext DependsOn<TModule>() where TModule : IRamshaModule
    {
        return DependsOn(typeof(TModule));
    }

    public RegisterContext DependsOn(Type moduleType)
    {
        if (!typeof(IRamshaModule).IsAssignableFrom(moduleType))
        {
            throw new ArgumentException($"Type must implement {nameof(IRamshaModule)}", nameof(moduleType));
        }

        _logger?.LogDebug("Module {Module} depends on {Dependency}", _currentModule.Name, moduleType.Name);

        _registry.AddDependency(_currentModule, moduleType);

        if (!_registry.IsRegistered(moduleType))
        {
            _logger?.LogDebug("Auto-registering dependency module: {Module}", moduleType.Name);
            RegisterModuleRecursive(moduleType);
        }
        else
        {
            _logger?.LogDebug("Skip Registration for module: {Module}, because it is already registered", moduleType.Name);
        }

        return this;
    }

    private void RegisterModuleRecursive(Type moduleType)
    {
        var instance = Activator.CreateInstance(moduleType) as IRamshaModule
            ?? throw new InvalidOperationException($"Cannot create instance of {moduleType.Name}");

        _registry.RegisterModule(moduleType, instance);

        _services.AddSingleton(moduleType, instance);

        var dependencyContext = new RegisterContext(
            _registry,
            moduleType,
            _services,
            _logger,
            _configuration);

        instance.Register(dependencyContext);

        _logger?.LogInformation("module: {Module} registered", moduleType.Name);

    }
}
