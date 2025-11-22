using Ramsha.Core.Modularity.Contexts;

namespace Ramsha;

public interface IRamshaModule
{
    void Register(RegisterContext context);
    Task RegisterAsync(RegisterContext context);
    void Prepare(PrepareContext context);
    Task PrepareAsync(PrepareContext context);
    Task BuildServicesAsync(BuildServicesContext context);
    void BuildServices(BuildServicesContext context);

}

public abstract class RamshaModule : IRamshaModule, IOnAppInit, IOnAppShutdown
{

    public virtual void BuildServices(BuildServicesContext context)
    {

    }

    public virtual Task BuildServicesAsync(BuildServicesContext context)
    {
        BuildServices(context);
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



    public virtual void Register(RegisterContext context)
    {

    }

    public Task RegisterAsync(RegisterContext context)
    {
        Register(context);
        return Task.CompletedTask;
    }

    public virtual void Prepare(PrepareContext context)
    {
    }

    public virtual Task PrepareAsync(PrepareContext context)
    {
        Prepare(context);
        return Task.CompletedTask;
    }


}







