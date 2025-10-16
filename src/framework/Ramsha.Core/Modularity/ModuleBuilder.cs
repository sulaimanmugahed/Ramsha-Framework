using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ramsha;

public class ModuleBuilder
{
    private readonly Type _moduleType;
    private readonly AppModulesContext _context;
    private readonly IServiceCollection _services;

    public ModuleBuilder(Type currentModuleType, IServiceCollection services, AppModulesContext context)
    {
        _services = services;
        _moduleType = currentModuleType;
        _context = context;
        _context.InsureModuleExist(_moduleType);
    }

    public IConfiguration GetConfiguration()
    {
        return _services.GetConfiguration();
    }

    public ModuleBuilder DependsOn<TModule>() where TModule : IRamshaModule
    {
        _context.InsureModuleExist(typeof(TModule));
        _context.AddDependency(_moduleType, typeof(TModule));
        return this;
    }
}

