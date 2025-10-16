namespace Ramsha;

public interface IRamshaModule
{
    void OnModuleCreating(ModuleBuilder moduleBuilder);
    Task OnAppConfiguringAsync(ConfigureContext context);
    void OnAppConfiguring(ConfigureContext context);

}

public abstract class RamshaModule : IRamshaModule, IOnAppInit, IOnAppShutdown
{
    protected internal ConfigureContext ConfigureContext
    {
        get
        {
            if (_serviceConfigurationContext == null)
            {
                throw new Exception($"{nameof(ConfigureContext)} is only available in configurations methods.");
            }

            return _serviceConfigurationContext;
        }
        internal set => _serviceConfigurationContext = value;
    }

    private ConfigureContext? _serviceConfigurationContext;
    public virtual void OnAppConfiguring(ConfigureContext context)
    {

    }

    public virtual Task OnAppConfiguringAsync(ConfigureContext context)
    {
        OnAppConfiguring(context);
        return Task.CompletedTask;
    }

    public virtual void OnAppInit(InitContext context)
    {

    }

    public virtual Task OnAppInitAsync(InitContext context)
    {
        OnAppInit(context);
        return Task.CompletedTask;
    }

    public virtual void OnAppShutdown(ShutdownContext context)
    {

    }

    public virtual Task OnAppShutdownAsync(ShutdownContext context)
    {
        OnAppShutdown(context);
        return Task.CompletedTask;
    }

    public virtual void OnModuleCreating(ModuleBuilder moduleBuilder)
    {

    }


}







