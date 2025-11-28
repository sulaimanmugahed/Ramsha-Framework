using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Ramsha.Authorization;

public class PermissionAuthorizationRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}
