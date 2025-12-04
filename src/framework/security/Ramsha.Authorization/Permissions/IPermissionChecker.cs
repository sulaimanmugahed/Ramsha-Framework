using System.Security.Claims;


namespace Ramsha.Authorization;

public interface IPermissionChecker
{
    Task<bool> IsAssignedAsync(string name);
    Task<bool> IsAssignedAsync(ClaimsPrincipal? claimsPrincipal, string permissionName);
}


