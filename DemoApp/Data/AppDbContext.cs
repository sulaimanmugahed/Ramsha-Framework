using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoApp.Entities;
using DemoApp.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Ramsha;
using Ramsha.Domain;
using Ramsha.EntityFrameworkCore;

namespace DemoApp;

[ConnectionString("MainDb")]
public class AppDbContext(DbContextOptions<AppDbContext> options)
: RamshaEFDbContext<AppDbContext>(options)
{
    public DbSet<RamshaIdentityUser> Users { get; set; }
    public DbSet<RamshaIdentityRole> Roles { get; set; }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }


    public bool IsPriceFilterEnabled => GlobalDataFilterManager.IsEnabled<IPrice>();

    public string Name => "";




    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
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


        //identity
        modelBuilder.Entity<RamshaIdentityUser>(entity =>
        {
            entity.HasMany(x => x.Claims).WithOne().HasForeignKey(uc => uc.UserId).IsRequired();

            entity.HasMany(x => x.Logins).WithOne().HasForeignKey(ul => ul.UserId).IsRequired();

            entity.HasMany(x => x.Tokens).WithOne().HasForeignKey(ut => ut.UserId).IsRequired();

            entity.HasMany(x => x.Roles).WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
        });

        modelBuilder.Entity<RamshaIdentityUserClaim<Guid>>(b =>
        {
            b.HasKey(uc => uc.Id);
        });

        modelBuilder.Entity<RamshaIdentityUserLogin<Guid>>(b =>
        {
            b.HasKey(l => new { l.LoginProvider, l.ProviderKey });

            b.Property(l => l.LoginProvider).HasMaxLength(128);
            b.Property(l => l.ProviderKey).HasMaxLength(128);
        });

        modelBuilder.Entity<RamshaIdentityUserToken<Guid>>(entity =>
      {
          entity.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

          entity.Property(t => t.LoginProvider).HasMaxLength(500);
          entity.Property(t => t.Name).HasMaxLength(500);
      });

        modelBuilder.Entity<RamshaIdentityRole>(entity =>
        {
            entity.HasKey(r => r.Id);

            entity.Property(u => u.Name).HasMaxLength(256);

            entity.HasMany(x => x.Users).WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();

            entity.HasMany(x => x.Claims).WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();
        });

        modelBuilder.Entity<RamshaIdentityRoleClaim<Guid>>(entity =>
       {
           entity.HasKey(x => x.Id);
       });

        modelBuilder.Entity<RamshaIdentityUserRole<Guid>>(b =>
        {
            b.HasKey(r => new { r.UserId, r.RoleId });
        });


    }
}
