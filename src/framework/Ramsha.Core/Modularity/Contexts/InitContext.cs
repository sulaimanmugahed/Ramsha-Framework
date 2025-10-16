using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha;

public class InitContext
{
    public IServiceProvider ServiceProvider { get; set; }

    public InitContext(IServiceProvider serviceProvider)
    {


        ServiceProvider = serviceProvider;
    }

}