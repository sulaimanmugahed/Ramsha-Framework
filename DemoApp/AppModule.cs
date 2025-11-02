using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoApp.Controllers;
using DemoApp.Entities;
using DemoModule;
using LiteBus.Commands;
using LiteBus.Extensions.Microsoft.DependencyInjection;
using LiteBus.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Ramsha;
using Ramsha.AspNetCore;
using Ramsha.AspNetCore.Mvc;
using Ramsha.Domain;
using Ramsha.EntityFrameworkCore;
using Ramsha.LocalMessaging;
using Ramsha.LocalMessaging.Abstractions;

namespace DemoApp;

public class AppModule : RamshaModule
{
    public override void OnCreating(ModuleBuilder moduleBuilder)
    {
        base.OnCreating(moduleBuilder);

        moduleBuilder
        .DependsOn<DemoModuleModule>()
        .DependsOn<AspNetCoreMvcModule>()
        .DependsOn<LocalMessagingModule>()
        .DependsOn<EntityFrameworkCoreModule>();

        moduleBuilder.PreConfigure<LocalMessagingOptions>(options =>
        {
            options.AddMessagesFromAssembly<AppModule>();
        });

    }

    public override void OnConfiguring(ConfigureContext context)
    {
        var configuration = context.Services.GetConfiguration();
        base.OnConfiguring(context);
        context.Services.AddScoped<IRamshaService, RamshaService>();
        context.Services.AddScoped<ITestService, TestService>();
       


        context.Services.Configure<RamshaDbContextOptions>(options =>
        {
            options.Configure(configurationContext =>
            {
                configurationContext.UseSqlServer();
            });
        });

        context.Services.AddRamshaDbContext<AppDbContext>(option =>
        {
            option.AddDefaultRepositories(true)
            .AddGlobalQueryFilterProvider<PriceFilterProvider>();
        });

        context.Services.Configure<ConnectionStringsOptions>(options =>
        {
            options.ConfigureAliases(builder =>
            {
                builder.Map("Default", ["MainDb", "Primary"]);
            });
        });


        context.Services.Configure<GlobalQueryFilterOptions>(options =>
       {
           options.DefaultStates[typeof(IFilter)] = new GlobalQueryFilterState(true);
       });

        context.Services.Configure<TestSetting>(configuration.GetSection(nameof(TestSetting)));

    }

    public override void OnInit(InitContext context)
    {
        base.OnInit(context);
    }
}
public static class RamshaDbContextConfigurationContextExtensions
{

    public static DbContextOptionsBuilder UseSqlServer(
         this RamshaDbContextConfigurationContext context,
          Action<SqlServerDbContextOptionsBuilder>? sqlServerOptionsAction = null)
    {
        if (context.ExistingConnection != null)
        {
            return context.DbContextOptions.UseSqlServer(context.ExistingConnection, optionsBuilder =>
            {
                optionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                sqlServerOptionsAction?.Invoke(optionsBuilder);
            });
        }
        else
        {
            return context.DbContextOptions.UseSqlServer(context.ConnectionString, optionsBuilder =>
            {
                optionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                sqlServerOptionsAction?.Invoke(optionsBuilder);
            });
        }
    }
}