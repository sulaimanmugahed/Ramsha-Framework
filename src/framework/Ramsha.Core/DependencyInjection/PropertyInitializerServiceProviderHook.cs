using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace Ramsha;

public sealed class PropertyInitializerServiceProviderHook : IServiceProviderHook
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PropertyInitializerServiceProviderHook> _logger;

    public PropertyInitializerServiceProviderHook(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _logger = serviceProvider.GetRequiredService<ILogger<PropertyInitializerServiceProviderHook>>();
    }

    public void ServiceResolved(Type serviceType, object service)
    {
        foreach (var prop in service.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.GetProperty))
        {
            var injectPropertyAttribute = prop.GetCustomAttribute<InjectableAttribute>();

            if (injectPropertyAttribute != null)
            {
                if (prop.GetValue(service) == null)
                {
                    _logger.LogDebug($"ServiceResolved: serviceType = {serviceType}, setting {prop.Name}");
                    var propertyService = _serviceProvider.GetRequiredKeyedService(prop.PropertyType, injectPropertyAttribute.ServiceKey);
                    prop.SetValue(service, propertyService);
                }
            }
        }
    }
}
