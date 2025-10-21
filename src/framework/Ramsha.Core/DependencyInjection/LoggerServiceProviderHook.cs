using Microsoft.Extensions.Logging;


namespace Ramsha;

public sealed class LoggerServiceProviderHook : IServiceProviderHook
{
    private readonly ILogger<LoggerServiceProviderHook> _logger;

    public LoggerServiceProviderHook(ILogger<LoggerServiceProviderHook> logger)
    {
        _logger = logger;
    }

    public void ServiceResolved(Type serviceType, object service)
    {
        _logger.LogDebug($"ServiceResolved: serviceType = {serviceType}, service = {service}");
    }
}
