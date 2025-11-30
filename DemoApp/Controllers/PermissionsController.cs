using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ramsha.AspNetCore.Mvc;
using Ramsha.Authorization;

namespace DemoApp.Controllers;

public record CreatePermissionRequest(string Group, string Permission, string[] Children);
public class PermissionsController(IPermissionStore store) : RamshaApiController
{
    [HttpGet(nameof(CreatePermission))]
    [Authorize("create")]
    public async Task<string> CreatePermission()
    {
        return "this is create";
    }

    [HttpGet(nameof(DeletePermission))]
    [Authorize("delete")]
    public async Task<string> DeletePermission()
    {
        return "this is delete";
    }

    [HttpPost(nameof(Grant))]
    public async Task Grant(string permissionName,string providerName, string providerKey, bool isGrant)
    {
        await store.GrantPermissionAsync(permissionName,providerName, providerKey, isGrant);
    }


}
