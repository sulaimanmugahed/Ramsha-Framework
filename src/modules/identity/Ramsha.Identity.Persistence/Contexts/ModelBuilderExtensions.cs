using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ramsha.Identity.Domain;

namespace Ramsha.Identity.Persistence;

public static class ModelBuilderExtensions
{
    public static void ConfigureDefaultIdentity(this ModelBuilder modelBuilder)
    {
        modelBuilder.ConfigureIdentity<RamshaIdentityUser, RamshaIdentityRole, Guid, RamshaIdentityUserRole<Guid>, RamshaIdentityRoleClaim<Guid>, RamshaIdentityUserClaim<Guid>, RamshaIdentityUserLogin<Guid>, RamshaIdentityUserToken<Guid>>();
    }
    public static void ConfigureIdentity<TUser, TRole, TId, TUserRole, TRoleClaim, TUserClaim, TUserLogin, TUserToken>(this ModelBuilder modelBuilder)
         where TId : IEquatable<TId>
     where TUser : RamshaIdentityUser<TId, TUserClaim, TUserRole, TUserLogin, TUserToken>
where TUserClaim : RamshaIdentityUserClaim<TId>
where TUserRole : RamshaIdentityUserRole<TId>
where TUserLogin : RamshaIdentityUserLogin<TId>
where TUserToken : RamshaIdentityUserToken<TId>
where TRole : RamshaIdentityRole<TId, TUserRole, TRoleClaim>
where TRoleClaim : RamshaIdentityRoleClaim<TId>
    {
        modelBuilder.Entity<TUser>(entity =>
            {
                entity.ToTable("RamshaIdentityUsers");
                entity.HasMany(x => x.Claims).WithOne().HasForeignKey(uc => uc.UserId).IsRequired();

                entity.HasMany(x => x.Logins).WithOne().HasForeignKey(ul => ul.UserId).IsRequired();

                entity.HasMany(x => x.Tokens).WithOne().HasForeignKey(ut => ut.UserId).IsRequired();

                entity.HasMany(x => x.Roles).WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
            });

        modelBuilder.Entity<TUserClaim>(entity =>
        {
            entity.ToTable("RamshaIdentityUserClaims");
            entity.HasKey(uc => uc.Id);
        });

        modelBuilder.Entity<TUserLogin>(b =>
        {
            b.ToTable("RamshaIdentityUserLogins");
            b.HasKey(l => new { l.LoginProvider, l.ProviderKey });
            b.Property(l => l.LoginProvider).HasMaxLength(128);
            b.Property(l => l.ProviderKey).HasMaxLength(128);
        });

        modelBuilder.Entity<TUserToken>(entity =>
      {
          entity.ToTable("RamshaIdentityUserTokens");
          entity.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
          entity.Property(t => t.LoginProvider).HasMaxLength(500);
          entity.Property(t => t.Name).HasMaxLength(500);
      });

        modelBuilder.Entity<TRole>(entity =>
        {
            entity.ToTable("RamshaIdentityRoles");

            entity.HasKey(r => r.Id);

            entity.Property(u => u.Name).HasMaxLength(256);

            entity.HasMany(x => x.Users).WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();

            entity.HasMany(x => x.Claims).WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();
        });

        modelBuilder.Entity<TRoleClaim>(entity =>
       {
           entity.ToTable("RamshaIdentityRoleClaims");
           entity.HasKey(x => x.Id);
       });

        modelBuilder.Entity<TUserRole>(b =>
        {
            b.ToTable("RamshaIdentityUserRoles");
            b.HasKey(r => new { r.UserId, r.RoleId });
        });
    }

}
