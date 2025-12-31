using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha.Common.Application;
using Ramsha.Permissions.Contracts;
using Ramsha.Permissions.Domain;
using Ramsha.Permissions.Shared;

namespace Ramsha.Permissions.Application;

public class PermissionsService(IPermissionManager permissionManager) : RamshaService, IPermissionsService
{
    public async Task<IRamshaResult> AssignAsync(string permissionName, string providerName, string providerKey)
    {
        return await permissionManager.AssignAsync(permissionName, providerName, providerKey);
    }

    public async Task<RamshaResult<List<PermissionInfo>>> GetAllAsync(string providerName, string providerKey)
    {
        return await permissionManager.GetAllAsync(providerName, providerKey);
    }

    public async Task<RamshaResult<PermissionInfo>> GetAsync(string permissionName, string providerName, string providerKey)
    {
        return await permissionManager.GetAsync(permissionName, providerName, providerKey);

    }

    public async Task<IRamshaResult> RevokeAsync(string permissionName, string providerName, string providerKey)
    {
        return await permissionManager.RevokeAsync(permissionName, providerName, providerKey);

    }
}
