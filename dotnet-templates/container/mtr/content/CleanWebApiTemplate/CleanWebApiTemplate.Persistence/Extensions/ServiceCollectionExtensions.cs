using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanWebApiTemplate.Persistence;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCleanWebApiTemplatePersistenceServices(this IServiceCollection services)
    {
        services.AddRamshaDbContext<CleanWebApiTemplateDbContext>(options =>
        {
            // EX: options.AddRepository<MyEntity,IMyRepo,MyRepo>();
        });

        return services;
    }
}
