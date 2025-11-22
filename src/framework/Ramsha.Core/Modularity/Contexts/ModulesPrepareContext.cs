using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Ramsha.Core.Modularity.Contexts;

public class PrepareContext(IServiceCollection services)
{
    public PrepareContext Configure<TOptions>(Action<TOptions> optionsAction)
      where TOptions : class
    {
        services.PrepareConfigure(optionsAction);
        return this;
    }

    public TOptions? Configure<TOptions>()
      where TOptions : class, new()
    {
        return services.ExecutePreparedOptions<TOptions>();
    }
}
