using System.Security.Claims;


namespace Ramsha.Authorization;

public interface IPermissionChecker
{
    Task<bool> HasPermissionAsync(string name);
    Task<bool> HasPermissionAsync(ClaimsPrincipal? claimsPrincipal, string permissionName);
}


