using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Ramsha;


namespace Ramsha.Domain;

public abstract class DbContextRegistrationOptionsBase : IDbContextRegistrationOptionsBaseBuilder
{
    public Type OriginalDbContextType { get; }

    public IServiceCollection Services { get; }

    public Dictionary<Type, Type?> ReplacedDbContextTypes { get; }

    public Type DefaultRepositoryDbContextType { get; protected set; }

    public Type? DefaultRepositoryImplementationType { get; private set; }

    public Type? DefaultRepositoryImplementationTypeWithoutKey { get; private set; }

    public bool RegisterDefaultRepositories { get; private set; }

    public bool IncludeAllEntitiesForDefaultRepositories { get; private set; }

    public Dictionary<Type, Type> CustomRepositories { get; }

    public List<Type> SpecifiedDefaultRepositories { get; }
    public List<Type> GlobalQueryFilterProviders { get; }


    public bool SpecifiedDefaultRepositoryTypes => DefaultRepositoryImplementationType != null && DefaultRepositoryImplementationTypeWithoutKey != null;

    protected DbContextRegistrationOptionsBase(Type originalDbContextType, IServiceCollection services)
    {
        OriginalDbContextType = originalDbContextType;
        Services = services;
        DefaultRepositoryDbContextType = originalDbContextType;
        CustomRepositories = [];
        ReplacedDbContextTypes = [];
        SpecifiedDefaultRepositories = [];
        GlobalQueryFilterProviders = [];
    }

    public IDbContextRegistrationOptionsBaseBuilder ReplaceDbContext<TOtherDbContext>()
    {
        return ReplaceDbContext(typeof(TOtherDbContext));
    }

    public IDbContextRegistrationOptionsBaseBuilder ReplaceDbContext<TOtherDbContext, TTargetDbContext>()
    {
        return ReplaceDbContext(typeof(TOtherDbContext), typeof(TTargetDbContext));
    }

    public IDbContextRegistrationOptionsBaseBuilder ReplaceDbContext(Type otherDbContextType, Type? targetDbContextType = null)
    {
        if (!otherDbContextType.IsAssignableFrom(OriginalDbContextType))
        {
            throw new Exception($"{OriginalDbContextType.AssemblyQualifiedName} should inherit/implement {otherDbContextType.AssemblyQualifiedName}!");
        }

        ReplacedDbContextTypes[otherDbContextType] = targetDbContextType;

        return this;
    }

    public IDbContextRegistrationOptionsBaseBuilder AddDefaultRepositories(bool includeAllEntities = false)
    {
        RegisterDefaultRepositories = true;
        IncludeAllEntitiesForDefaultRepositories = includeAllEntities;

        return this;
    }

    public IDbContextRegistrationOptionsBaseBuilder AddDefaultRepositories(Type defaultRepositoryDbContextType, bool includeAllEntities = false)
    {
        if (!defaultRepositoryDbContextType.IsAssignableFrom(OriginalDbContextType))
        {
            throw new Exception($"{OriginalDbContextType.AssemblyQualifiedName} should inherit/implement {defaultRepositoryDbContextType.AssemblyQualifiedName}!");
        }

        DefaultRepositoryDbContextType = defaultRepositoryDbContextType;

        return AddDefaultRepositories(includeAllEntities);
    }

    public IDbContextRegistrationOptionsBaseBuilder AddDefaultRepositories<TDefaultRepositoryDbContext>(bool includeAllEntities = false)
    {
        return AddDefaultRepositories(typeof(TDefaultRepositoryDbContext), includeAllEntities);
    }

    public IDbContextRegistrationOptionsBaseBuilder AddDefaultRepository<TEntity>()
    {
        return AddDefaultRepository(typeof(TEntity));
    }

    public IDbContextRegistrationOptionsBaseBuilder AddDefaultRepository(Type entityType)
    {
        if (SpecifiedDefaultRepositories.Any())
        {
            SpecifiedDefaultRepositories.Add(entityType);
        }

        return this;
    }

    public IDbContextRegistrationOptionsBaseBuilder AddGlobalQueryFilterProvider(Type filterProviderType)
    {
        if (!GlobalQueryFilterProviders.Any(x => x == filterProviderType))
        {
            GlobalQueryFilterProviders.Add(filterProviderType);
        }

        return this;
    }

    public IDbContextRegistrationOptionsBaseBuilder AddGlobalQueryFilterProvider<TProvider>()
    where TProvider : IGlobalQueryFilterProvider
    {
        AddGlobalQueryFilterProvider(typeof(TProvider));
        return this;
    }

    public IDbContextRegistrationOptionsBaseBuilder AddRepository<TEntity, TRepository>()
    {
        AddCustomRepository(typeof(TEntity), typeof(TRepository));

        return this;
    }

    public IDbContextRegistrationOptionsBaseBuilder SetDefaultRepositoryClasses(
        Type repositoryImplementationType,
        Type repositoryImplementationTypeWithoutKey
        )
    {


        DefaultRepositoryImplementationType = repositoryImplementationType;
        DefaultRepositoryImplementationTypeWithoutKey = repositoryImplementationTypeWithoutKey;

        return this;
    }

    private void AddCustomRepository(Type entityType, Type repositoryType)
    {
        if (!typeof(IEntity).IsAssignableFrom(entityType))
        {
            throw new Exception($"Given entityType is not an entity: {entityType.AssemblyQualifiedName}. It must implement {typeof(IEntity<>).AssemblyQualifiedName}.");
        }

        if (!typeof(IRepository).IsAssignableFrom(repositoryType))
        {
            throw new Exception($"Given repositoryType is not a repository: {entityType.AssemblyQualifiedName}. It must implement {typeof(IRepository<>).AssemblyQualifiedName}.");
        }

        CustomRepositories[entityType] = repositoryType;
    }

}
