namespace Ramsha;

public interface IRamshaHooksManager
{
    Task Initialize(InitContext context);
    Task Shutdown(ShutdownContext context);
}
