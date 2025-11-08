using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ramsha.Domain;
using Ramsha.EntityFrameworkCore;
using Ramsha.Identity.Domain;

namespace Ramsha.Identity.Persistence;

[ConnectionString(RamshaIdentityDbContextConstants.ConnectionStringName)]

public interface IIdentityDbContext : IIdentityDbContext<RamshaIdentityUser, RamshaIdentityRole, Guid, RamshaIdentityUserRole<Guid>, RamshaIdentityRoleClaim<Guid>, RamshaIdentityUserClaim<Guid>, RamshaIdentityUserLogin<Guid>, RamshaIdentityUserToken<Guid>>
{

}

public interface IEFIdentityDbContext<TUser> : IIdentityDbContext<TUser, RamshaIdentityRole<Guid>, Guid, RamshaIdentityUserRole<Guid>, RamshaIdentityRoleClaim<Guid>, RamshaIdentityUserClaim<Guid>, RamshaIdentityUserLogin<Guid>, RamshaIdentityUserToken<Guid>>
where TUser : RamshaIdentityUser
{

}


public interface IEFIdentityDbContext<TUser, TRole> : IIdentityDbContext<TUser, TRole,  Guid, RamshaIdentityUserRole<Guid>, RamshaIdentityRoleClaim<Guid>, RamshaIdentityUserClaim<Guid>, RamshaIdentityUserLogin<Guid>, RamshaIdentityUserToken<Guid>>
where TUser : RamshaIdentityUser
where TRole : RamshaIdentityRole
{

}

public interface IEFIdentityDbContext<TUser, TRole, TId> : IIdentityDbContext<TUser, TRole, TId, RamshaIdentityUserRole<TId>, RamshaIdentityRoleClaim<TId>, RamshaIdentityUserClaim<TId>, RamshaIdentityUserLogin<TId>, RamshaIdentityUserToken<TId>>
where TId : IEquatable<TId>
where TUser : RamshaIdentityUser<TId>
where TRole : RamshaIdentityRole<TId>
{

}


public interface IIdentityDbContext<TUser, TRole, TId, TUserRole, TRoleClaim, TUserClaim, TUserLogin, TUserToken>
: IRamshaEFDbContext
     where TId : IEquatable<TId>
     where TUser : RamshaIdentityUser<TId, TUserClaim, TUserRole, TUserLogin, TUserToken>
where TUserClaim : RamshaIdentityUserClaim<TId>
where TUserRole : RamshaIdentityUserRole<TId>
where TUserLogin : RamshaIdentityUserLogin<TId>
where TUserToken : RamshaIdentityUserToken<TId>
where TRole : RamshaIdentityRole<TId, TUserRole, TRoleClaim>
where TRoleClaim : RamshaIdentityRoleClaim<TId>
{
    DbSet<TUser> Users { get; }
    DbSet<TRole> Roles { get; }
    DbSet<TUserClaim> UserClaims { get; }
    DbSet<TUserRole> UserRoles { get; }
    DbSet<TUserLogin> UserLogins { get; }
    DbSet<TUserToken> UserTokens { get; }
    DbSet<TRoleClaim> RoleClaims { get; }
}
