
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ramsha.Domain;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionRepositoryExtensions
{
    public static IServiceCollection RegisterCustomRepository(
        this IServiceCollection services,
        Type repositoryType
    )
    {
        services.AddTransient(repositoryType);
        return services;
    }
    public static IServiceCollection RegisterCustomRepository(
    this IServiceCollection services,
    Type repositoryInterface,
    Type repositoryImplementation
)
    {
        services.AddTransient(repositoryInterface, repositoryImplementation);
        return services;
    }
    public static IServiceCollection RegisterDefaultRepository(
        this IServiceCollection services,
        Type entityType,
        Type repositoryImplementationType,
        bool replaceExisting = false)
    {
        var repositoryInterface = typeof(IRepository<>).MakeGenericType(entityType);
        if (repositoryInterface.IsAssignableFrom(repositoryImplementationType))
        {
            RegisterRepository(services, repositoryInterface, repositoryImplementationType, replaceExisting);
        }

        var primaryKeyType = EntityHelper.FindPrimaryKeyType(entityType);
        if (primaryKeyType != null)
        {
            var repositoryInterfaceWithPk = typeof(IRepository<,>).MakeGenericType(entityType, primaryKeyType);
            if (repositoryInterfaceWithPk.IsAssignableFrom(repositoryImplementationType))
            {
                RegisterRepository(services, repositoryInterfaceWithPk, repositoryImplementationType, replaceExisting);

            }
        }

        return services;
    }

    private static void RegisterRepository(
        IServiceCollection services,
        Type serviceType,
        Type implementationType,
        bool replaceExisting)
    {
        var descriptor = ServiceDescriptor.Transient(serviceType, implementationType);

        if (replaceExisting)
        {
            services.Replace(descriptor);
        }
        else
        {
            services.TryAdd(descriptor);
        }
    }





}

