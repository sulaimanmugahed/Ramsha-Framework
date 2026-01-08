using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ramsha.EntityFrameworkCore;

public class EntityFrameworkCoreInitHookContributor : IInitHookContributor
{
    public async Task OnInitialize(InitContext context)
    {
        var provider = context.ServiceProvider;
        var dbContexts = provider.GetServices<IRamshaEFDbContext>();
        if (dbContexts != null)
        {
            foreach (var db in dbContexts)
            {
                if (db.Database.GetPendingMigrations().Count() > 0)
                {
                    await db.Database.MigrateAsync();
                }
            }
        }
    }
}
