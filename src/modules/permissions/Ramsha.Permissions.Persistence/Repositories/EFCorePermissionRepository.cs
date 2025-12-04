using Ramsha.EntityFrameworkCore;
using Ramsha.Permissions.Domain;

namespace Ramsha.Permissions.Persistence;

public class EFCorePermissionRepository :
 EFCoreRepository<PermissionsDbContext, Permission, Guid>,
 IPermissionRepository
{

}

