using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Ramsha.Autofac;

public class RamshaAutofacServiceProviderFactory : IServiceProviderFactory<ContainerBuilder>
{
    private readonly ContainerBuilder _builder;
    private IServiceCollection _services = default!;

    public RamshaAutofacServiceProviderFactory(ContainerBuilder builder)
    {
        _builder = builder;
    }

    public ContainerBuilder CreateBuilder(IServiceCollection services)
    {
        _services = services;
        _builder.BuildContainer(services);
        return _builder;
    }

    public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
    {
        return new AutofacServiceProvider(containerBuilder.Build());
    }


}
