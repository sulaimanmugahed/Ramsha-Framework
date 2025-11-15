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
where TUser : RamshaIdentityUser, new()
{

}


public interface IEFIdentityDbContext<TUser, TRole> : IIdentityDbContext<TUser, TRole, Guid, RamshaIdentityUserRole<Guid>, RamshaIdentityRoleClaim<Guid>, RamshaIdentityUserClaim<Guid>, RamshaIdentityUserLogin<Guid>, RamshaIdentityUserToken<Guid>>
where TUser : RamshaIdentityUser, new()
where TRole : RamshaIdentityRole, new()
{

}

public interface IEFIdentityDbContext<TUser, TRole, TId> : IIdentityDbContext<TUser, TRole, TId, RamshaIdentityUserRole<TId>, RamshaIdentityRoleClaim<TId>, RamshaIdentityUserClaim<TId>, RamshaIdentityUserLogin<TId>, RamshaIdentityUserToken<TId>>
where TId : IEquatable<TId>
where TUser : RamshaIdentityUser<TId>, new()
where TRole : RamshaIdentityRole<TId>, new()
{

}


public interface IIdentityDbContext<TUser, TRole, TId, TUserRole, TRoleClaim, TUserClaim, TUserLogin, TUserToken>
: IRamshaEFDbContext
     where TId : IEquatable<TId>
     where TUser : RamshaIdentityUser<TId, TUserClaim, TUserRole, TUserLogin, TUserToken>, new()
where TUserClaim : RamshaIdentityUserClaim<TId>, new()
where TUserRole : RamshaIdentityUserRole<TId>, new()
where TUserLogin : RamshaIdentityUserLogin<TId>, new()
where TUserToken : RamshaIdentityUserToken<TId>, new()
where TRole : RamshaIdentityRole<TId, TUserRole, TRoleClaim>, new()
where TRoleClaim : RamshaIdentityRoleClaim<TId>, new()
{
    DbSet<TUser> Users { get; }
    DbSet<TRole> Roles { get; }
    DbSet<TUserClaim> UserClaims { get; }
    DbSet<TUserRole> UserRoles { get; }
    DbSet<TUserLogin> UserLogins { get; }
    DbSet<TUserToken> UserTokens { get; }
    DbSet<TRoleClaim> RoleClaims { get; }
}
