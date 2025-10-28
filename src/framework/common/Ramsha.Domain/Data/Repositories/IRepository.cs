using System.Linq.Expressions;

namespace Ramsha.Domain;

public interface IRepository
{

}

public interface IRepository<TEntity> : IRepository
where TEntity : IEntity
{
    Task<TEntity?> CreateAsync(TEntity entity);
    Task<List<TEntity>> GetListAsync();
    Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> expression);
}

public interface IRepository<TEntity, TId> : IRepository<TEntity>
where TEntity : IEntity<TId>
{
    Task<TEntity?> FindAsync(TId id);
}




