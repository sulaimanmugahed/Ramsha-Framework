using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha;

public class ShutdownContext
{
    public IServiceProvider ServiceProvider { get; }

    public ShutdownContext(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }
}