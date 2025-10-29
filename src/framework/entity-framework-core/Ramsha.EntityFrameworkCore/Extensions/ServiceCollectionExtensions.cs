
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ramsha.Domain;
using Ramsha.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRamshaDbContext<TDbContext>(
        this IServiceCollection services,
           Action<IDbContextRegistrationOptionsBaseBuilder>? optionsBuilder = null)
        where TDbContext : RamshaDbContext<TDbContext>
    {
        services.AddMemoryCache();
        services.AddTransient<TDbContext>();
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

        return services;
    }


}
