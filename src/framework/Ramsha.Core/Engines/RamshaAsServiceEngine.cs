using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ramsha;

public class RamshaAsServiceEngine : RamshaEngine, IExternalRamshaEngine
{
    public IEnumerable<Type> StarterModulesTypes;
    private readonly HashSet<Type> _processedModules;
    private readonly List<Action<PrepareContext>>? _prepareActions;


    public RamshaAsServiceEngine(
        IServiceCollection services,
        IEnumerable<Type> starterModulesTypes,
        List<Action<PrepareContext>>? prepareActions = null)
        : base(services)
    {
        StarterModulesTypes = starterModulesTypes;
        _prepareActions = prepareActions;
        _processedModules = new();
        services.AddSingleton<IExternalRamshaEngine>(this);
    }

    protected override async Task PrepareModules(PrepareContext context, IReadOnlyList<IModuleDescriptor> modules)
    {
        await base.PrepareModules(context, modules);
        _prepareActions?.ForEach(a => a(context));
    }

    protected override IReadOnlyList<IModuleDescriptor> LoadModules(IServiceCollection services)
    {
        var logger = services.GetBootstrapLogger<RamshaEngine>();

        // Filter out already processed modules
        var unprocessedStarters = StarterModulesTypes
            .Where(type => !_processedModules.Contains(type))
            .ToArray();

        if (unprocessedStarters.Length == 0)
        {
            logger.LogInformation("All starter modules have already been processed");
            return Array.Empty<IModuleDescriptor>();
        }


        var loader = new RamshaAsServiceModuleLoader();
        var descriptors = loader.LoadModules(services, unprocessedStarters, logger);

        foreach (var starterType in unprocessedStarters)
        {
            _processedModules.Add(starterType);
        }

        return descriptors;
    }

    public async Task Initialize(IServiceProvider serviceProvider)
    {
        InternalSetServiceProvider(serviceProvider);
        await base.InitializeAsync();
    }

    public void SetServiceProvider(IServiceProvider serviceProvider)
    {
        if (ServiceProvider != null)
        {
            if (ServiceProvider != serviceProvider)
            {
                throw new Exception("Service provider was already set before to another service provider instance.");
            }

            return;
        }

        InternalSetServiceProvider(serviceProvider);
    }
}
