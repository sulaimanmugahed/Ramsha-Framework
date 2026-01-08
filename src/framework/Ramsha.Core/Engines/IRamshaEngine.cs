namespace Ramsha;

public interface IRamshaEngine : IModuleContainer
{
    IServiceProvider ServiceProvider { get; }
    Task ConfigureAsync();
    Task ShutdownAsync();
}
