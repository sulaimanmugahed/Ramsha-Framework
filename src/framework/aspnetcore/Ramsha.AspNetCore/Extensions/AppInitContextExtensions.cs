using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ramsha.AspNetCore;

public static class AppInitContextExtensions
{
    public static IAppPipeline<IApplicationBuilder> GetAppPipelineBuilder(this InitContext context)
    {
        return context.ServiceProvider.GetRequiredService<IAppPipeline<IApplicationBuilder>>();
    }
}
