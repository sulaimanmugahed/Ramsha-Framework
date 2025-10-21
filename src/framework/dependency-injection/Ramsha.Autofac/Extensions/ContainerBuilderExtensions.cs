using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;

namespace Ramsha.Autofac.Extensions;

public static class ContainerBuilderExtensions
{
    public static void ConfigureAutofacContainer(this ContainerBuilder builder)
    {
        builder.RegisterType<RamshaInjectPropertySelector>()
               .As<IPropertySelector>()
               .SingleInstance();

        builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces()
               .PropertiesAutowired(new RamshaInjectPropertySelector(false));
    }
}
