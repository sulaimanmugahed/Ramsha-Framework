using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Ramsha.EntityFrameworkCore;

public abstract class RamshaDbContext<TDbContext>(DbContextOptions<TDbContext> dbContextOptions)
: DbContext(dbContextOptions)
where TDbContext : RamshaDbContext<TDbContext>
{

    [Injectable]
    public IServiceProvider ServiceProvider { get; set; } = default!;
    public IOptions<RamshaDbContextOptions> Options => ServiceProvider.GetLazyRequiredService<IOptions<RamshaDbContextOptions>>().Value;
}







