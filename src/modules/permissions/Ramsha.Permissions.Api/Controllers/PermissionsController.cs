
using Microsoft.AspNetCore.Mvc;
using Ramsha.AspNetCore.Mvc;
using Ramsha.Permissions.Contracts;
using Ramsha.Permissions.Shared;

namespace Ramsha.Permissions.Api;

public class PermissionsController(IPermissionsService permissionsService) : RamshaApiController
{
    [HttpPost]
    public async Task<IActionResult> AssignAsync(string permissionName, string providerName, string providerKey)
    {
        return RamshaResult(await permissionsService.AssignAsync(permissionName, providerName, providerKey));
    }
    [HttpGet("all")]
    public async Task<ActionResult<List<PermissionInfo>>> GetAllAsync(string providerName, string providerKey)
    {
        return RamshaResult(await permissionsService.GetAllAsync(providerName, providerKey));
    }
    [HttpGet]
    public async Task<ActionResult<PermissionInfo>> GetAsync(string permissionName, string providerName, string providerKey)
    {
        return RamshaResult(await permissionsService.GetAsync(permissionName, providerName, providerKey));

    }
    [HttpDelete]
    public async Task<IRamshaResult> RevokeAsync(string permissionName, string providerName, string providerKey)
    {
        return await permissionsService.RevokeAsync(permissionName, providerName, providerKey);
    }
}
