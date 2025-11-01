using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ramsha;
using Ramsha.Domain;
using Ramsha.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRamshaDbContext<TDbContext>(
        this IServiceCollection services,
           Action<IDbContextRegistrationOptionsBaseBuilder>? optionsBuilder = null)
        where TDbContext : RamshaEFDbContext<TDbContext>
    {
        services.AddMemoryCache();

        services.TryAddTransient(sp =>
{
    var instance = ActivatorUtilities.CreateInstance<TDbContext>(sp);
    instance.ServiceProvider = sp;
    return instance;
});

        services.TryAddTransient<IRamshaEFDbContext>(sp => sp.GetRequiredService<TDbContext>());
        services.TryAddTransient<IEFDbContext>(sp => sp.GetRequiredService<TDbContext>());

        var options = new EfDbContextRegistrationOptions(typeof(TDbContext), services);

        optionsBuilder?.Invoke(options);
        services.TryAddTransient(RamshaDbContextOptionsFactory.Create<TDbContext>);

        foreach (var entry in options.ReplacedDbContextTypes)
        {
            var originalDbContextType = entry.Key;
            var targetDbContextType = entry.Value ?? typeof(TDbContext);

            services.Replace(ServiceDescriptor.Transient(originalDbContextType, sp =>
            {
                var dbContextType = sp.GetRequiredService<IEfDbContextTypeProvider>().GetDbContextType(originalDbContextType);
                return sp.GetRequiredService(dbContextType);
            }));

            services.Configure<RamshaDbContextOptions>(opts =>
            {
                opts.DbContextReplacements[originalDbContextType] = targetDbContextType;
            });
        }

        new EfRepositoryRegistrar(options).AddRepositories();
        new EFGlobalQueryFilterRegistrar(options).RegisterFilters();

        return services;
    }


}

public class RamshaDbContextOptionsExtension<TDbContext> : IDbContextOptionsExtension
where TDbContext : RamshaEFDbContext<TDbContext>
{
    public void ApplyServices(IServiceCollection services)
    {


    }

    public void Validate(IDbContextOptions options)
    {
    }

    public DbContextOptionsExtensionInfo Info => new RamshaOptionsExtensionInfo(this);

    private class RamshaOptionsExtensionInfo : DbContextOptionsExtensionInfo
    {
        public RamshaOptionsExtensionInfo(IDbContextOptionsExtension extension)
            : base(extension)
        {
        }

        public override bool IsDatabaseProvider => false;

        public override int GetServiceProviderHashCode()
        {
            return 0;
        }

        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other)
        {
            return other is RamshaOptionsExtensionInfo;
        }

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        {
        }

        public override string LogFragment => "RamshaOptionsExtension";
    }
}
