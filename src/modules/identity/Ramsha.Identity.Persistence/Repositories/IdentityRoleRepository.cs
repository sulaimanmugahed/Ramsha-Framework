using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha.EntityFrameworkCore;
using Ramsha.Identity.Domain;

namespace Ramsha.Identity.Persistence;

public class EFIdentityRoleRepository<TDbContext, TUser, TRole, TId, TUserRole, TRoleClaim, TUserClaim, TUserLogin, TUserToken>
(IDbContextProvider<TDbContext> dbContextProvider)
: EFCoreRepository<TDbContext, TRole, TId>(dbContextProvider),
IIdentityRoleRepository<TRole, TId>
 where TId : IEquatable<TId>
where TUser : RamshaIdentityUser<TId, TUserClaim, TUserRole, TUserLogin, TUserToken>
 where TUserClaim : RamshaIdentityUserClaim<TId>
 where TUserRole : RamshaIdentityUserRole<TId>
 where TUserLogin : RamshaIdentityUserLogin<TId>
where TUserToken : RamshaIdentityUserToken<TId>
where TRoleClaim : RamshaIdentityRoleClaim<TId>
where TRole : RamshaIdentityRole<TId, TUserRole, TRoleClaim>
where TDbContext : IIdentityDbContext<TUser, TRole, TId, TUserRole, TRoleClaim, TUserClaim, TUserLogin, TUserToken>
{

}
