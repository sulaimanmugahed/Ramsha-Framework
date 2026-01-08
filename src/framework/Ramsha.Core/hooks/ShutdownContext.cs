
namespace Ramsha;

public class ShutdownContext
{
    public IServiceProvider ServiceProvider { get; }

    public ShutdownContext(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }
}