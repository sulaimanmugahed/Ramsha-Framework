using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Ramsha;

public interface IModuleLoader
{
    IModuleDescriptor[] LoadModules(IServiceCollection services, Type startupModuleType, ILogger logger);
    IModuleDescriptor[] LoadModules(IServiceCollection services, IRamshaModule startupModuleInstance, ILogger logger);
}
