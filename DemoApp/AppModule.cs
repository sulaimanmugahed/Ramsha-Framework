using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoModule;
using LiteBus.Extensions.Microsoft.DependencyInjection;
using LiteBus.Queries;
using Ramsha;
using Ramsha.AspNetCore;
using Ramsha.AspNetCore.Mvc;
using Ramsha.LocalMessaging;

namespace DemoApp;

public class AppModule : RamshaModule
{
    public override void OnCreating(ModuleBuilder moduleBuilder)
    {
        base.OnCreating(moduleBuilder);

        moduleBuilder
        .DependsOn<DemoModuleModule>()
        .DependsOn<AspNetCoreMvcModule>()
        .DependsOn<LocalMessagingModule>();
    }

    public override void OnConfiguring(ConfigureContext context)
    {
        base.OnConfiguring(context);
        context.Services.AddScoped<IRamshaService, RamshaService>();
        context.Services.AddScoped<ITestService, TestService>();
        context.Services.AddLiteBus(options =>
      {

          options.AddQueryModule(builder =>
          {
              builder.RegisterFromAssembly(typeof(TestQuery).Assembly);
          });
      });

    }

    public override void OnInit(InitContext context)
    {
        base.OnInit(context);

    }

}
