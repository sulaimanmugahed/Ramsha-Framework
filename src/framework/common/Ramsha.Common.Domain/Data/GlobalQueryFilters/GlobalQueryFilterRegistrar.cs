using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Ramsha.Common.Domain;

public abstract class GlobalQueryFilterRegistrar<TOptions>
 where TOptions : DbContextRegistrationOptionsBase
{
    public TOptions Options { get; }

    public GlobalQueryFilterRegistrar(TOptions options)
    {
        Options = options;
    }

    public void RegisterFilters()
    {
       

        foreach (var filterProvider in Options.GlobalQueryFilterProviders)
        {
            Options.Services.AddSingleton(filterProvider);
            Options.Services.AddSingleton(
                GetGlobalQueryProviderInterface(Options.OriginalDbContextType),
                filterProvider);

        }
    }

    protected abstract Type GetGlobalQueryProviderInterface(Type dbContextType);

}