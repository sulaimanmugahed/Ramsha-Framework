using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha.Authorization;
using Ramsha.Permissions.Shared;

namespace Ramsha.Permissions.Domain;

public interface IPermissionManager
{
    Task<PermissionInfo> GetAsync(string name, string providerName, string providerKey);
    Task<List<PermissionInfo>> GetAllAsync(string providerName, string providerKey);
    Task<IRamshaResult> AssignAsync(string name, string providerName, string providerKey);
    Task<IRamshaResult> RevokeAsync(string name, string providerName, string providerKey);

}
