using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha.Authorization;
using Ramsha.UnitOfWork.Abstractions;

namespace Ramsha.Permissions.Domain.Services;

public class PersistPermissionStore(IPermissionRepository assignmentRepository) : IPermissionStore
{
    public async Task<bool> IsAssignedAsync(string permissionName, string providerName, string providerKey)
    {
        return await assignmentRepository
        .FindAsync(x =>
         x.Name == permissionName &&
         x.ProviderName == providerName &&
        x.ProviderKey == providerKey) is not null;
    }


}
