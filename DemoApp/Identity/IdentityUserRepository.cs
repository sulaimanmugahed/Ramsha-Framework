using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha.EntityFrameworkCore;

namespace DemoApp.Identity;


public class IdentityUserRepository<TUser, TId, TUserRole, TRoleClaim, TUserClaim, TUserLogin, TUserToken, TRole>(IDbContextProvider<AppDbContext> dbContextProvider)
: EFCoreRepository<AppDbContext, TUser, TId>(dbContextProvider), IIdentityUserRepository<TUser, TId, TUserRole, TRoleClaim, TUserClaim, TUserLogin, TUserToken, TRole>
 where TId : IEquatable<TId>
where TUser : RamshaIdentityUser<TId, TUserClaim, TUserRole, TUserLogin, TUserToken>
 where TUserClaim : RamshaIdentityUserClaim<TId>
 where TUserRole : RamshaIdentityUserRole<TId>
 where TUserLogin : RamshaIdentityUserLogin<TId>
where TUserToken : RamshaIdentityUserToken<TId>
{
}


