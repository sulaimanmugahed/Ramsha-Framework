using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ramsha.EntityFrameworkCore;
using Ramsha.Permissions.Domain;

namespace Ramsha.Permissions.Persistence;

public sealed class PermissionsDbContext(DbContextOptions<PermissionsDbContext> options)
: RamshaEFDbContext<PermissionsDbContext>(options)
{
    public DbSet<Permission> Permissions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ConfigurePermissions();
    }
}
