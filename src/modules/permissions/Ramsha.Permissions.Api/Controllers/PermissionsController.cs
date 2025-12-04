using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ramsha.AspNetCore.Mvc;
using Ramsha.Permissions.Contracts;
using Ramsha.Permissions.Shared;

namespace Ramsha.Permissions.Api;

[Route("permissions")]
public class PermissionsController(IPermissionsService permissionsService) : RamshaApiController
{
    [HttpPost]
    public Task<RamshaResult> AssignAsync(string permissionName, string providerName, string providerKey)
    {
        return permissionsService.AssignAsync(permissionName, providerName, providerKey);
    }
    [HttpGet("all")]
    public Task<RamshaResult<List<PermissionInfo>>> GetAllAsync(string providerName, string providerKey)
    {
        return permissionsService.GetAllAsync(providerName, providerKey);
    }
    [HttpGet]
    public Task<RamshaResult<PermissionInfo>> GetAsync(string permissionName, string providerName, string providerKey)
    {
        return permissionsService.GetAsync(permissionName, providerName, providerKey);

    }
    [HttpDelete]
    public Task<RamshaResult> RevokeAsync(string permissionName, string providerName, string providerKey)
    {
        return permissionsService.RevokeAsync(permissionName, providerName, providerKey);

    }
}
