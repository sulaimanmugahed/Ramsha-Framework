using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Ramsha.EntityFrameworkCore;

namespace Microsoft.EntityFrameworkCore;

public static class DbContextOptionsExtensions
{
    public static DbContextOptionsBuilder AddAbpDbContextOptionsExtension<TDbContext>(this DbContextOptionsBuilder optionsBuilder)
    where TDbContext : RamshaEFDbContext<TDbContext>
    {
        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(new RamshaDbContextOptionsExtension<TDbContext>());
        return optionsBuilder;
    }

}
