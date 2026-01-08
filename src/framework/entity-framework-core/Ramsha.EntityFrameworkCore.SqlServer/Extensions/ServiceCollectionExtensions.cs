using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Ramsha.EntityFrameworkCore.SqlServer;

public static class ServiceCollectionExtensions
{
    public static RamshaBuilder AddEFSqlServerProvider(this RamshaBuilder ramsha)
    {
        ramsha.AddModule<EntityFrameworkCoreSqlServerModule>();
        return ramsha;
    }
}
