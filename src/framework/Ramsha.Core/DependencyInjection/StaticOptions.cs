using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Ramsha;

public class StaticOptions<TOptions> : IOptionsSnapshot<TOptions>
where TOptions : class
{
    private readonly TOptions _options;

    public StaticOptions(TOptions options)
    {
        _options = options;
    }

    public TOptions Value
    {
        get
        {
            return _options;
        }
    }

    public TOptions Get(string name)
    {
        return _options;
    }
}
