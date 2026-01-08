

namespace Ramsha;

public class InitContext
{
    public IServiceProvider ServiceProvider { get; set; }

    public InitContext(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

}