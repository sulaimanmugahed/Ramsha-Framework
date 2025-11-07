
using Microsoft.EntityFrameworkCore;
using Ramsha.Domain;
using Ramsha.EntityFrameworkCore;
using Ramsha.Identity.Domain;

namespace Ramsha.Identity.Persistence;

[ConnectionString(RamshaIdentityDbContextConstants.ConnectionStringName)]
public class IdentityDbContext(DbContextOptions<IdentityDbContext> options)
: RamshaEFDbContext<IdentityDbContext>(options), IIdentityDbContext
{
    public DbSet<RamshaIdentityUser> Users { get; set; }

    public DbSet<RamshaIdentityRole> Roles { get; set; }

    public DbSet<RamshaIdentityUserClaim<Guid>> UserClaims { get; set; }

    public DbSet<RamshaIdentityUserRole<Guid>> UserRoles { get; set; }

    public DbSet<RamshaIdentityUserLogin<Guid>> UserLogins { get; set; }

    public DbSet<RamshaIdentityUserToken<Guid>> UserTokens { get; set; }

    public DbSet<RamshaIdentityRoleClaim<Guid>> RoleClaims { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ConfigureDefaultIdentity();
    }
}
