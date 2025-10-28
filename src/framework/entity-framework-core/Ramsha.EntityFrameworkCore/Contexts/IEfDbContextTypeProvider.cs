using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Ramsha.EntityFrameworkCore;



public interface IEfDbContextTypeProvider
{
    Type GetDbContextType(Type dbContextType);
}

public class EfDbContextTypeProvider : IEfDbContextTypeProvider
{
    private readonly RamshaDbContextOptions _options;

    public EfDbContextTypeProvider(IOptions<RamshaDbContextOptions> options)
    {
        _options = options.Value;
    }

    public virtual Type GetDbContextType(Type dbContextType)
    {
        return _options.GetReplacedTypeOrSelf(dbContextType);
    }
}
