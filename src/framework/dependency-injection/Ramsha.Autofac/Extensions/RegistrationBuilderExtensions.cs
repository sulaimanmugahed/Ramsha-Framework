using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Ramsha.Autofac;

public static class RegistrationBuilderExtensions
{

    public static ContainerBuilder BuildContainer(this ContainerBuilder builder, IServiceCollection services)
    {
        builder.Populate(services);

        var assemblies = RamshaAssemblyHelpers.GetRamshaAssemblies();

        builder.RegisterAssemblyTypes(assemblies.ToArray())
              .Where(t => typeof(IHasPropertyInjection).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
              .AsImplementedInterfaces()
              .PropertiesAutowired(new RamshaInjectPropertySelector(true));
        return builder;
    }


}

