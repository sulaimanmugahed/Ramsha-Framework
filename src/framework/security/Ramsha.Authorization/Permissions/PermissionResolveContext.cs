using System.Security.Claims;


namespace Ramsha.Authorization;

public class PermissionResolveContext
{
    public PermissionDefinition Permission { get; }
    public ClaimsPrincipal? Principal { get; }
    public PermissionResolveContext(
         PermissionDefinition permission,
        ClaimsPrincipal? principal)
    {
        Permission = permission;
        Principal = principal;
    }
}


