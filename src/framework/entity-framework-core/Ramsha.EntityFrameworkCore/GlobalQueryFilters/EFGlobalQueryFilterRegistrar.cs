using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Ramsha.Common.Domain;

namespace Ramsha.EntityFrameworkCore;

public class EFGlobalQueryFilterRegistrar : GlobalQueryFilterRegistrar<EfDbContextRegistrationOptions>
{
    public EFGlobalQueryFilterRegistrar(EfDbContextRegistrationOptions options)
       : base(options)
    {
    }

    protected override Type GetGlobalQueryProviderInterface(Type dbContextType)
    {
        return typeof(IEFGlobalQueryFilterProvider<>).MakeGenericType(dbContextType);
    }
}