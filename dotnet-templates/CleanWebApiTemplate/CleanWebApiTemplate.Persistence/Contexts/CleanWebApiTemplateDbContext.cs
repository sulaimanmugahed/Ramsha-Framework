
using Microsoft.EntityFrameworkCore;
using Ramsha.Common.Domain;
using Ramsha.EntityFrameworkCore;

namespace CleanWebApiTemplate.Persistence;

[ConnectionString(CleanWebApiTemplateDbContextConstants.ConnectionStringName)]
public class CleanWebApiTemplateDbContext(
    DbContextOptions<CleanWebApiTemplateDbContext> options
    )
: RamshaEFDbContext<CleanWebApiTemplateDbContext>(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ConfigureCleanWebApiTemplate();
    }
}
