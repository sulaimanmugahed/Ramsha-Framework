using Microsoft.Extensions.DependencyInjection;

namespace Ramsha;

public interface IRamshaAppEngine :
IRamshaEngine,
IAppInfoAccessor,
IDisposable
{
    Type StartupModuleType { get; }
    IServiceCollection Services { get; }

}
