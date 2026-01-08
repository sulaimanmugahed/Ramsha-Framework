namespace Ramsha;

public interface IInternalRamshaEngine : IRamshaEngine
{
    IServiceProvider CreateServiceProvider();
    Task Initialize();
}
