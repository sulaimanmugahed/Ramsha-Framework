using Microsoft.Extensions.DependencyInjection;

namespace Ramsha.Common.Domain;




public abstract class RepositoryRegistrar<TOptions>
    where TOptions : DbContextRegistrationOptionsBase
{
    public TOptions Options { get; }

    protected RepositoryRegistrar(TOptions options)
    {
        Options = options;
    }

    public virtual void AddRepositories()
    {
        RegisterCustomRepositories();
        RegisterDefaultRepositories();
        RegisterSpecifiedDefaultRepositories();
    }

    protected virtual void RegisterCustomRepositories()
    {
        foreach (var customRepository in Options.CustomRepositories)
        {
            if (customRepository.Value.SelfRegister)
            {
                //register the repository itself
                Options.Services.RegisterCustomRepository(customRepository.Value.RepositoryType);
            }

            //register the repository  interfaces
            foreach (var interfaceType in customRepository.Value.RepositoryInterfacesTypes)
            {
                Options.Services.RegisterCustomRepository(interfaceType, customRepository.Value.RepositoryType);
            }

            // replace the Generic Repositories 
            Options.Services.RegisterDefaultRepository(customRepository.Key, customRepository.Value.RepositoryType, replaceExisting: true);
        }
    }

    protected virtual void RegisterDefaultRepositories()
    {
        if (!Options.RegisterDefaultRepositories)
        {
            return;
        }

        foreach (var entityType in GetEntityTypes(Options.OriginalDbContextType))
        {
            if (!ShouldRegisterDefaultRepositoryFor(entityType))
            {
                continue;
            }

            RegisterDefaultRepository(entityType);
        }
    }

    protected virtual void RegisterSpecifiedDefaultRepositories()
    {
        foreach (var entityType in Options.SpecifiedDefaultRepositories)
        {
            if (!Options.CustomRepositories.ContainsKey(entityType))
            {
                RegisterDefaultRepository(entityType);
            }
        }
    }

    protected virtual void RegisterDefaultRepository(Type entityType)
    {
        Options.Services.RegisterDefaultRepository(
            entityType,
            GetDefaultRepositoryImplementationType(entityType)
        );
    }

    protected virtual Type GetDefaultRepositoryImplementationType(Type entityType)
    {
        var primaryKeyType = EntityHelper.FindPrimaryKeyType(entityType);

        if (primaryKeyType == null)
        {
            return Options.SpecifiedDefaultRepositoryTypes
                ? Options.DefaultRepositoryImplementationTypeWithoutKey!.MakeGenericType(entityType)
                : GetRepositoryType(Options.DefaultRepositoryDbContextType, entityType);
        }

        return Options.SpecifiedDefaultRepositoryTypes
            ? Options.DefaultRepositoryImplementationType!.MakeGenericType(entityType, primaryKeyType)
            : GetRepositoryType(Options.DefaultRepositoryDbContextType, entityType, primaryKeyType);
    }

    protected virtual bool ShouldRegisterDefaultRepositoryFor(Type entityType)
    {
        if (!Options.RegisterDefaultRepositories)
        {
            return false;
        }

        if (Options.CustomRepositories.ContainsKey(entityType))
        {
            return false;
        }

        if (!Options.IncludeAllEntitiesForDefaultRepositories && !typeof(IAggregateRoot<>).IsAssignableFrom(entityType))
        {
            return false;
        }

        return true;
    }

    protected abstract IEnumerable<Type> GetEntityTypes(Type dbContextType);

    protected abstract Type GetRepositoryType(Type dbContextType, Type entityType);

    protected abstract Type GetRepositoryType(Type dbContextType, Type entityType, Type primaryKeyType);
}

