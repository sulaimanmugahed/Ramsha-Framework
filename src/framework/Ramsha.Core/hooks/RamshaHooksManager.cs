using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Ramsha;

public class RamshaHooksManager : IRamshaHooksManager
{
    private readonly IEnumerable<IInitHookContributor> _initContributors;
    private readonly IEnumerable<IShutdownHookContributor> _shutdownContributors;

    public RamshaHooksManager(
     IOptions<RamshaHooksOptions> options,
     IServiceProvider serviceProvider)
    {
        _initContributors = options.Value
           .InitHookContributors
           .Select(serviceProvider.GetRequiredService)
           .Cast<IInitHookContributor>()
           .ToArray();
        _shutdownContributors = options.Value
           .ShutdownHookContributors
           .Select(serviceProvider.GetRequiredService)
           .Cast<IShutdownHookContributor>()
           .ToArray();
    }
    public async Task Initialize(InitContext context)
    {
        foreach (var contributor in _initContributors)
        {
            try
            {
                await contributor.OnInitialize(context);
            }
            catch (Exception ex)
            {
                throw new RamshaException($"An error occurred during init hook running", ex);
            }
        }
    }

    public async Task Shutdown(ShutdownContext context)
    {
        foreach (var contributor in _shutdownContributors)
        {
            try
            {
                await contributor.OnShutdown(context);
            }
            catch (Exception ex)
            {
                throw new RamshaException($"An error occurred during Shutdown hook running", ex);
            }
        }
    }
}
