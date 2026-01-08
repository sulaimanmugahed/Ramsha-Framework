namespace Ramsha;

public interface IExternalRamshaEngine : IRamshaEngine
{
    void SetServiceProvider(IServiceProvider serviceProvider);
    Task Initialize(IServiceProvider serviceProvider);
}
