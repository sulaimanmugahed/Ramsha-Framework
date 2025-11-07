using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Ramsha;
using Ramsha.Domain;
using Ramsha.EntityFrameworkCore;
using Ramsha.Identity.Domain;
using Ramsha.Identity.Persistence;

namespace DemoApp;

[ConnectionString("MainDb")]
public class AppDbContext(DbContextOptions<AppDbContext> options)
: RamshaEFDbContext<AppDbContext>(options), IIdentityDbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public bool IsPriceFilterEnabled => GlobalDataFilterManager.IsEnabled<IPrice>();
    public string Name => "";

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
        modelBuilder.Entity<Category>(e =>
        {
            e.HasKey(c => c.Id);
        });
        modelBuilder.Entity<Product>(b =>
        {
            b.HasKey(p => p.Id);
            b.Property(p => p.Name).IsRequired().HasMaxLength(200);
            b.Property(p => p.Price).HasColumnType("decimal(18,2)");

            b.HasOne<Category>()
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.CategoryId);
        });

    }
}
