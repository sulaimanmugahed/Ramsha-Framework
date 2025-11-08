
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Ramsha.Domain;

namespace Ramsha.EntityFrameworkCore;

public class EFCoreRepository<TDbContext, TEntity>(IDbContextProvider<TDbContext> dbContextProvider) : IRepository<TEntity>
where TDbContext : IEFDbContext
where TEntity : class, IEntity
{
    protected virtual async Task<TDbContext> GetDbContextAsync()
    => await dbContextProvider.GetDbContextAsync();

    public virtual async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] includes)
    {
        var context = await GetDbContextAsync();
        var query = context.Set<TEntity>().AsQueryable();
        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        return await query.FirstOrDefaultAsync(expression);
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

    public async Task<List<TEntity>> GetListAsync(params Expression<Func<TEntity, object>>[] includes)
    {
        var context = await GetDbContextAsync();
        var query = context.Set<TEntity>().AsQueryable();
        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        return await query.ToListAsync();
    }

    public async Task DeleteAsync(TEntity entity)
    {
        var context = await GetDbContextAsync();
        context.Remove(entity);
    }

    public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> criteria, params Expression<Func<TEntity, object>>[] includes)
    {
        var context = await GetDbContextAsync();
        var query = context.Set<TEntity>().AsQueryable();
        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        return await query.Where(criteria).ToListAsync();
    }

    public async Task<int> GetCountAsync()
    {
        var context = await GetDbContextAsync();
        return await context.Set<TEntity>().CountAsync();
    }

    public async Task<int> GetCountAsync(Expression<Func<TEntity, bool>> criteria)
    {
        var context = await GetDbContextAsync();
        return await context.Set<TEntity>().Where(criteria).CountAsync();
    }
}


public class EFCoreRepository<TDbContext, TEntity, TId>(IDbContextProvider<TDbContext> dbContextProvider) : EFCoreRepository<TDbContext, TEntity>(dbContextProvider), IRepository<TEntity, TId>
where TDbContext : IEFDbContext
where TId:IEquatable<TId>
where TEntity : class, IEntity<TId>

{
    public async Task<TEntity?> FindAsync(TId id)
    {
        var context = await GetDbContextAsync();
        return await context.Set<TEntity>().FindAsync(id);
    }

    public async Task<TEntity?> FindAsync(TId id, params Expression<Func<TEntity, object>>[] includes)
    {
        var context = await GetDbContextAsync();
        var query = context.Set<TEntity>().AsQueryable();
        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        return await query.FirstOrDefaultAsync();
    }

    public async Task<bool> DeleteAsync(TId id)
    {
        var context = await GetDbContextAsync();
        var entity = await context.Set<TEntity>().FindAsync(id);
        if (entity is null)
        {
            return false;
        }

        context.Remove(entity);
        return true;
    }

    public async Task<bool> IsExist(TId id)
    {
        var context = await GetDbContextAsync();
        var existEntity = await context.Set<TEntity>().FindAsync(id);
        return existEntity is not null;
    }
}