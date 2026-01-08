
namespace Ramsha;

public interface IRamshaModule
{
    void Register(RegisterContext context);
    void Prepare(PrepareContext context);
    Task PrepareAsync(PrepareContext context);
    Task BuildServicesAsync(BuildServicesContext context);
    void BuildServices(BuildServicesContext context);

}

public abstract class RamshaModule : IRamshaModule
{
    public virtual void BuildServices(BuildServicesContext context)
    {

    }

    public virtual Task BuildServicesAsync(BuildServicesContext context)
    {
        BuildServices(context);
        return Task.CompletedTask;
    }

    public virtual void Register(RegisterContext context)
    {

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







