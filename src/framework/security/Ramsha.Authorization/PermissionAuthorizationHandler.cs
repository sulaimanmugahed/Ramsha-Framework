using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Ramsha.Authorization;

namespace Ramsha.Authorization;

public class PermissionAuthorizationHandler(IPermissionChecker permissionChecker)
   : AuthorizationHandler<PermissionAuthorizationRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionAuthorizationRequirement requirement)
    {
        if (context.User.Identity?.IsAuthenticated == true
            && await permissionChecker.IsAssignedAsync(context.User, requirement.Permission))
        {
            context.Succeed(requirement);
        }
    }
}