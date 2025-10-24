using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha;

public class PreConfigureActionList<TOptions> : List<Action<TOptions>>
{
    public void Configure(TOptions options)
    {
        foreach (var action in this)
        {
            action(options);
        }
    }

    public TOptions Configure()
    {
        var options = Activator.CreateInstance<TOptions>();
        Configure(options);
        return options;
    }
}
