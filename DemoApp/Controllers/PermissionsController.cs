using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoApp.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ramsha.AspNetCore.Mvc;
using Ramsha.Authorization;

namespace DemoApp.Controllers;

public record CreatePermissionRequest(string Group, string Permission, string[] Children);
public class PermissionsTestController : RamshaApiController
{
    [HttpGet(nameof(CreatePermission))]
    [Authorize(ProductPermissions.Manage.Create)]
    public async Task<string> CreatePermission()
    {
        return "this is create";
    }

    [HttpGet(nameof(DeletePermission))]
    [Authorize(ProductPermissions.Manage.Delete)]
    public async Task<string> DeletePermission()
    {
        return "this is delete";
    }


}
