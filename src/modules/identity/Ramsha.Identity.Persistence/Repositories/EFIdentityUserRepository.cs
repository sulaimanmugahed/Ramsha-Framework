using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha.EntityFrameworkCore;
using Ramsha.Identity.Domain;

namespace Ramsha.Identity.Persistence;

public class EFIdentityUserRepository<TDbContext, TUser, TRole, TId, TUserRole, TRoleClaim, TUserClaim, TUserLogin, TUserToken>(IDbContextProvider<IIdentityDbContext<TUser, TRole, TId, TUserRole, TRoleClaim, TUserClaim, TUserLogin, TUserToken>> dbContextProvider)
: EFCoreRepository<IIdentityDbContext<TUser, TRole, TId, TUserRole, TRoleClaim, TUserClaim, TUserLogin, TUserToken>, TUser, TId>(dbContextProvider), IIdentityUserRepository<TUser, TId>
 where TId : IEquatable<TId>
where TUser : RamshaIdentityUser<TId, TUserClaim, TUserRole, TUserLogin, TUserToken>, new()
 where TUserClaim : RamshaIdentityUserClaim<TId>, new()
 where TUserRole : RamshaIdentityUserRole<TId>, new()
 where TUserLogin : RamshaIdentityUserLogin<TId>, new()
where TUserToken : RamshaIdentityUserToken<TId>, new()
where TRoleClaim : RamshaIdentityRoleClaim<TId>, new()
where TRole : RamshaIdentityRole<TId, TUserRole, TRoleClaim>, new()
where TDbContext : IIdentityDbContext<TUser, TRole, TId, TUserRole, TRoleClaim, TUserClaim, TUserLogin, TUserToken>
{
}

