using Microsoft.EntityFrameworkCore;
using Ramsha.UnitOfWork;

namespace Ramsha.EntityFrameworkCore;

public class EfCoreDatabaseApi : IDatabaseApi, ISupportsSavingChanges
{
    public IEFDbContext DbContext { get; }

    public EfCoreDatabaseApi(IEFDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return DbContext.SaveChangesAsync(cancellationToken);
    }
}
