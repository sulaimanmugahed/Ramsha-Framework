using Microsoft.Extensions.DependencyInjection;


namespace Ramsha.Domain;

public interface IDbContextRegistrationOptionsBaseBuilder
{
    IServiceCollection Services { get; }
    IDbContextRegistrationOptionsBaseBuilder AddDefaultRepositories(bool includeAllEntities = false);

    IDbContextRegistrationOptionsBaseBuilder AddDefaultRepositories<TDefaultRepositoryDbContext>(bool includeAllEntities = false);

    IDbContextRegistrationOptionsBaseBuilder AddDefaultRepository<TEntity>();

    IDbContextRegistrationOptionsBaseBuilder AddDefaultRepository(Type entityType);

    IDbContextRegistrationOptionsBaseBuilder AddRepository<TEntity, TRepository>();

    IDbContextRegistrationOptionsBaseBuilder SetDefaultRepositoryClasses(Type repositoryImplementationType, Type repositoryImplementationTypeWithoutKey);

    IDbContextRegistrationOptionsBaseBuilder ReplaceDbContext<TOtherDbContext>();

    IDbContextRegistrationOptionsBaseBuilder ReplaceDbContext<TOtherDbContext, TTargetDbContext>();

    IDbContextRegistrationOptionsBaseBuilder ReplaceDbContext(Type otherDbContextType, Type? targetDbContextType = null);
}