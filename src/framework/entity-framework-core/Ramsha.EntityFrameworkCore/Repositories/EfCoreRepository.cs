
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Ramsha.Domain;

namespace Ramsha.EntityFrameworkCore;

public class EfCoreRepository<TDbContext, TEntity>(IDbContextProvider<TDbContext> dbContextProvider) : IRepository<TEntity>
where TDbContext : DbContext
where TEntity : class, IEntity
{
    protected virtual async Task<TDbContext> GetDbContextAsync()
    => await dbContextProvider.GetDbContextAsync();
    public virtual async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> expression)
    {
        var context = await GetDbContextAsync();
        return await context.Set<TEntity>().FirstOrDefaultAsync(expression);
    }

    public virtual async Task<List<TEntity>> GetListAsync()
    {
        var context = await GetDbContextAsync();
        return await context.Set<TEntity>().ToListAsync();
    }

    public virtual async Task<TEntity?> CreateAsync(TEntity entity)
    {
        var context = await GetDbContextAsync();
        var entry = await context.Set<TEntity>().AddAsync(entity);
        return entry.Entity;

    }
}


public class EfCoreRepository<TDbContext, TEntity, TId>(IDbContextProvider<TDbContext> dbContextProvider) : EfCoreRepository<TDbContext, TEntity>(dbContextProvider), IRepository<TEntity, TId>
where TDbContext : DbContext
where TEntity : class, IEntity<TId>

{
    public async Task<TEntity?> FindAsync(TId id)
    {
        var context = await GetDbContextAsync();
        return await context.Set<TEntity>().FindAsync(id);
    }
}