using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Ramsha.EntityFrameworkCore;

public interface IDbContextProvider<TDbContext>
where TDbContext : IEFDbContext
{
    Task<TDbContext> GetDbContextAsync();

}


