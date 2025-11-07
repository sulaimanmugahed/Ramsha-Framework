using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha.EntityFrameworkCore;
using Ramsha.Identity.Domain;

namespace Ramsha.Identity.Persistence;

public class EFIdentityUserRepository<TDbContext, TUser, TRole, TId, TUserRole, TRoleClaim, TUserClaim, TUserLogin, TUserToken>(IDbContextProvider<TDbContext> dbContextProvider)
: EFCoreRepository<TDbContext, TUser, TId>(dbContextProvider), IIdentityUserRepository<TUser, TRole, TId, TUserRole, TRoleClaim, TUserClaim, TUserLogin, TUserToken>
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

