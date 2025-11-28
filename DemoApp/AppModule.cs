using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DemoApp.Controllers;
using DemoApp.Data;
using DemoApp.Entities;
using DemoModule;
using LiteBus.Commands;
using LiteBus.Extensions.Microsoft.DependencyInjection;
using LiteBus.Queries;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Ramsha;
using Ramsha.Account.Api;
using Ramsha.Account.Application;
using Ramsha.Account.Contracts;
using Ramsha.AspNetCore;
using Ramsha.AspNetCore.Mvc;
using Ramsha.Authorization;
using Ramsha.Common.Domain;
using Ramsha.Core.Modularity.Contexts;
using Ramsha.EntityFrameworkCore;
using Ramsha.EntityFrameworkCore.SqlServer;
using Ramsha.Identity.Api;
using Ramsha.Identity.Application;
using Ramsha.Identity.AspNetCore;
using Ramsha.Identity.Contracts;

using Ramsha.Identity.Domain;
using Ramsha.Identity.Persistence;
using Ramsha.Identity.Shared;
using Ramsha.LocalMessaging;
using Ramsha.LocalMessaging.Abstractions;
using Ramsha.Security.Claims;

namespace DemoApp;

public class AppModule : RamshaModule
{
    public override void Register(RegisterContext context)
    {
        base.Register(context);

        context
        .DependsOn<AuthorizationModule>()
        .DependsOn<IdentityApplicationModule>()
        .DependsOn<AccountApplicationModule>()
        .DependsOn<IdentityPersistenceModule>()
        .DependsOn<EntityFrameworkCoreSqlServerModule>()
        .DependsOn<IdentityAspNetCoreModule>()
        .DependsOn<IdentityApiModule>()
        .DependsOn<AccountApiModule>();

    }
    public override void Prepare(PrepareContext context)
    {
        base.Prepare(context);
        context.Configure<LocalMessagingOptions>(options =>
       {
           options.AddMessagesFromAssembly<AppModule>();
       });

        context.Configure<RamshaIdentityTypesOptions>(options =>
        {
            options.UserType = typeof(AppIdentityUser);
        });
        context.Configure<RamshaIdentityContractsOptions>(options =>
        {
            options.ReplaceDto<CreateRamshaIdentityUserDto, CreateAppUserDto>();
            options.ReplaceDto<UpdateRamshaIdentityUserDto, UpdateAppUserDto>();
            options.ReplaceUserService<AppUserService>();
        });

        context.Configure<RamshaAccountContractsOptions>(options =>
        {
            options.ReplaceDto<RamshaRegisterDto, AppRegisterDto>();
            options.ReplaceAccountService<AppAccountService>();
        });
    }

    public override void BuildServices(BuildServicesContext context)
    {
        var configuration = context.Services.GetConfiguration();
        base.BuildServices(context);


        context.Services.AddTransient<ITestService, TestService>();





        context.Services.AddRamshaDbContext<AppDbContext>(option =>
        {
            option.AddDefaultRepositories(true)
               .AddGlobalQueryFilterProvider<PriceFilterProvider>()

             .AddRepository<Product, IProductRepository, ProductRepository>()
             //  .ReplaceDbContext<IIdentityDbContext>()
             ;
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
           options.DefaultStates[typeof(IPrice)] = new GlobalQueryFilterState(true);
       });

        context.Services.Configure<TestSetting>(configuration.GetSection(nameof(TestSetting)));

        context.Services.Configure<RamshaClaimsPrincipalFactoryOptions>(options =>
        {
            options.Transformers.Add<DemoClaimsTransformer>();
            options.Transformers.AddBefore<DemoClaimsTransformer, DemoClaimsTransformerBefore>();
            options.Transformers.AddAfter<DemoClaimsTransformer, DemoClaimsTransformerAfter>();

        });
    }

    public override void OnInit(InitContext context)
    {
        base.OnInit(context);
    }
}


