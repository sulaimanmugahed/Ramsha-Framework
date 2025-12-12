using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Ramsha.Cli.Core;
using Ramsha.Cli.Infrastructure;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterInfrastructure(this IServiceCollection services, bool includeGenerators = true)
    {
        services.AddSingleton<ITemplateRenderer, ScribanRenderer>();
        services.AddSingleton<ITemplateRegistry<EmbeddedTemplateInfo>, EmbeddedTemplateRegistry>();
        services.AddSingleton<ITemplateRegistry<DotnetTemplateInfo>, DotnetTemplateRegistry>();


        if (includeGenerators)
        {
            services.AddTemplateGenerators();
        }

        return services;
    }
    public static IServiceCollection AddTemplateGenerators(this IServiceCollection services)
    {
        services.AddTransient(typeof(ITemplateGenerator<>), typeof(EmbeddedTemplateGenerator<>));
        services.AddTransient(typeof(ITemplateGenerator), typeof(EmbeddedTemplateGenerator));
        services.AddTransient(typeof(IEmbeddedTemplateGenerator<>), typeof(EmbeddedTemplateGenerator<>));
        services.AddTransient(typeof(IDotnetTemplateGenerator), typeof(DotnetTemplateGenerator));

        return services;
    }
}