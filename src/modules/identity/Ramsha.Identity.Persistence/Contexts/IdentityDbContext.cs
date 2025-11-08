
using Microsoft.EntityFrameworkCore;
using Ramsha.Domain;
using Ramsha.EntityFrameworkCore;
using Ramsha.Identity.Domain;

namespace Ramsha.Identity.Persistence;

[ConnectionString(RamshaIdentityDbContextConstants.ConnectionStringName)]
public class IdentityDbContext<TUser, TRole, TId, TUserRole, TRoleClaim, TUserClaim, TUserLogin, TUserToken>(DbContextOptions<IdentityDbContext<TUser, TRole, TId, TUserRole, TRoleClaim, TUserClaim, TUserLogin, TUserToken>> options)
: RamshaEFDbContext<IdentityDbContext<TUser, TRole, TId, TUserRole, TRoleClaim, TUserClaim, TUserLogin, TUserToken>>(options), IIdentityDbContext<TUser, TRole, TId, TUserRole, TRoleClaim, TUserClaim, TUserLogin, TUserToken>
     where TId : IEquatable<TId>
     where TUser : RamshaIdentityUser<TId, TUserClaim, TUserRole, TUserLogin, TUserToken>
where TUserClaim : RamshaIdentityUserClaim<TId>
where TUserRole : RamshaIdentityUserRole<TId>
where TUserLogin : RamshaIdentityUserLogin<TId>
where TUserToken : RamshaIdentityUserToken<TId>
where TRole : RamshaIdentityRole<TId, TUserRole, TRoleClaim>
where TRoleClaim : RamshaIdentityRoleClaim<TId>
{
    public DbSet<TUser> Users { get; set; }

    public DbSet<TRole> Roles { get; set; }

    public DbSet<TUserClaim> UserClaims { get; set; }

    public DbSet<TUserRole> UserRoles { get; set; }

    public DbSet<TUserLogin> UserLogins { get; set; }

    public DbSet<TUserToken> UserTokens { get; set; }

    public DbSet<TRoleClaim> RoleClaims { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ConfigureIdentity<TUser, TRole, TId, TUserRole, TRoleClaim, TUserClaim, TUserLogin, TUserToken>();
    }
}
