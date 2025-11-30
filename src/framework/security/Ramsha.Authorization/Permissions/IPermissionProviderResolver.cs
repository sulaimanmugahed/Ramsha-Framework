namespace Ramsha.Authorization;

public interface IPermissionProviderResolver
{
    string GetProviderName();
    Task<PermissionStatus> ResolveAsync(PermissionResolveContext context);
}


