using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ramsha.Common.Domain;
using Ramsha.EntityFrameworkCore;
using Ramsha.SettingsManagement.Domain;

namespace Ramsha.SettingsManagement.Persistence;

[ConnectionString(SettingsManagementDbContextConstants.ConnectionStringName)]
public interface ISettingsManagementDbContext : IRamshaEFDbContext
{
    DbSet<Setting> Settings { get; }
}
public class SettingsManagementDbContext(
    DbContextOptions<SettingsManagementDbContext> options)
    : RamshaEFDbContext<SettingsManagementDbContext>(options), ISettingsManagementDbContext
{
    public DbSet<Setting> Settings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ConfigureSettingsManagement();
    }
}
