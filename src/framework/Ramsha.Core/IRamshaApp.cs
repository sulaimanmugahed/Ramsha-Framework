using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Ramsha;

public interface IRamshaApp :
    IModuleContainer,
    IAppInfoAccessor,
    IDisposable
{
    Type StartupModuleType { get; }
    IServiceCollection Services { get; }
    IServiceProvider ServiceProvider { get; }
    Task ConfigureServicesAsync();
    Task ShutdownAsync();
    void Shutdown();
}
public interface IAppInfoAccessor
{
    string? ApplicationName { get; }
    string InstanceId { get; }
}
