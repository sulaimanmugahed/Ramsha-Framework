
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ramsha.EntityFrameworkCore;

public class RamshaDbContextConfigurationContext
{
    public IServiceProvider ServiceProvider { get; }

    public string ConnectionString { get; }

    public string? ConnectionStringName { get; }

    public DbConnection? ExistingConnection { get; }

    public DbContextOptionsBuilder DbContextOptions { get; protected set; }

    public RamshaDbContextConfigurationContext(
         string connectionString,
        IServiceProvider serviceProvider,
        string? connectionStringName,
        DbConnection? existingConnection)
    {
        ConnectionString = connectionString;
        ServiceProvider = serviceProvider;
        ConnectionStringName = connectionStringName;
        ExistingConnection = existingConnection;

        DbContextOptions = new DbContextOptionsBuilder()
            .UseLoggerFactory(serviceProvider.GetRequiredService<ILoggerFactory>())
            .UseApplicationServiceProvider(serviceProvider);
    }
}

public class RamshaDbContextConfigurationContext<TDbContext> : RamshaDbContextConfigurationContext
    where TDbContext : RamshaEFDbContext<TDbContext>
{
    public new DbContextOptionsBuilder<TDbContext> DbContextOptions => (DbContextOptionsBuilder<TDbContext>)base.DbContextOptions;

    public RamshaDbContextConfigurationContext(
        string connectionString,
         IServiceProvider serviceProvider,
        string? connectionStringName,
        DbConnection? existingConnection)
        : base(
              connectionString,
              serviceProvider,
              connectionStringName,
              existingConnection)
    {
        base.DbContextOptions = new DbContextOptionsBuilder<TDbContext>()
            .UseLoggerFactory(serviceProvider.GetRequiredService<ILoggerFactory>())
            .UseApplicationServiceProvider(serviceProvider);
    }

}



