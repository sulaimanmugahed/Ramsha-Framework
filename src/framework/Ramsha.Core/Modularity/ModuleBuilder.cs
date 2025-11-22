// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;

// namespace Ramsha;

// public class ModuleBuilder
// {
//     private readonly Type _moduleType;
//     private readonly ModulesRegisterContext _context;
//     private readonly IServiceCollection _services;

//     public ModuleBuilder(Type moduleType, IServiceCollection services, ModulesRegisterContext context)
//     {
//         _services = services;
//         _moduleType = moduleType;
//         _context = context;
//         _context.InsureModuleExist(_moduleType);
//     }

//     public ModuleBuilder Configure<TOptions>(Action<TOptions> optionsAction)
//          where TOptions : class
//     {
//         _services.PrepareConfigure(optionsAction);
//         return this;
//     }

//     public TOptions? Configure<TOptions>()
//       where TOptions : class, new()
//     {
//         return _services.ExecutePreparedOptions<TOptions>();
//     }

//     public IConfiguration GetConfiguration()
//     {
//         return _services.GetConfiguration();
//     }
// }

