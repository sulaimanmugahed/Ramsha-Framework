namespace Ramsha;

public interface IRamshaModule
{
    void OnCreating(ModuleBuilder moduleBuilder);
    Task OnConfiguringAsync(ConfigureContext context);
    void OnConfiguring(ConfigureContext context);

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
    public virtual void OnConfiguring(ConfigureContext context)
    {

    }

    public virtual Task OnConfiguringAsync(ConfigureContext context)
    {
        OnConfiguring(context);
        return Task.CompletedTask;
    }

    public virtual void OnInit(InitContext context)
    {

    }

    public virtual Task OnInitAsync(InitContext context)
    {
        OnInit(context);
        return Task.CompletedTask;
    }

    public virtual void OnShutdown(ShutdownContext context)
    {

    }

    public virtual Task OnShutdownAsync(ShutdownContext context)
    {
        OnShutdown(context);
        return Task.CompletedTask;
    }

    public virtual void OnCreating(ModuleBuilder moduleBuilder)
    {

    }


}







