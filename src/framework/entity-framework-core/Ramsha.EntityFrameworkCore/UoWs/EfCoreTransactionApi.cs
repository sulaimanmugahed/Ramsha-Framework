using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Ramsha.UnitOfWork;

namespace Ramsha.EntityFrameworkCore;

public class EfCoreTransactionApi : ITransactionApi, ISupportsRollback
{
    public List<DbContext> AttendedDbContexts { get; }
    public EfCoreTransactionApi(IDbContextTransaction dbContextTransaction)
    {
        DbContextTransaction = dbContextTransaction;
        AttendedDbContexts = [];
    }
    public IDbContextTransaction DbContextTransaction { get; }
    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        foreach (var dbContext in AttendedDbContexts)
        {
            if (dbContext.HasRelationalTransactionManager() &&
                dbContext.Database.GetDbConnection() == DbContextTransaction.GetDbTransaction().Connection)
            {
                continue;
            }

            await dbContext.Database.CommitTransactionAsync();
        }
        await DbContextTransaction.CommitAsync(cancellationToken);
    }

    public void Dispose()
    {
        DbContextTransaction.Dispose();
    }

    public async Task RollbackAsync(CancellationToken cancellationToken)
    {
        foreach (var dbContext in AttendedDbContexts)
        {
            if (dbContext.HasRelationalTransactionManager() &&
                dbContext.Database.GetDbConnection() == DbContextTransaction.GetDbTransaction().Connection)
            {
                continue;
            }

            await dbContext.Database.RollbackTransactionAsync();
        }
        await DbContextTransaction.RollbackAsync(cancellationToken);
    }
}
