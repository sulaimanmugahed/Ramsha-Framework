namespace Ramsha.Authorization;

public interface IPermissionValueResolver
{
    string GetProviderName();
    Task<PermissionStatus> ResolveAsync(PermissionResolveContext context);
}

public abstract class PermissionValueResolver(string providerName) : IPermissionValueResolver
{
    public string GetProviderName()
    {
        return providerName;
    }

    public  abstract Task<PermissionStatus> ResolveAsync(PermissionResolveContext context);
}


