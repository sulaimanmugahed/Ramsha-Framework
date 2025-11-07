using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DemoApp.Controllers;
using DemoApp.Data;
using DemoApp.Entities;
using DemoApp.Identity;
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
using Ramsha.AspNetCore;
using Ramsha.AspNetCore.Mvc;
using Ramsha.Domain;
using Ramsha.EntityFrameworkCore;
using Ramsha.LocalMessaging;
using Ramsha.LocalMessaging.Abstractions;
using Ramsha.Security.Claims;

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
            .AddGlobalQueryFilterProvider<PriceFilterProvider>()
             .AddRepository<Product, IProductRepository, ProductRepository>();
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


        // context.Services.AddScoped<IUserStore<RamshaIdentityUser>, RamshaUserStore>();
        // context.Services.AddScoped<IRoleStore<RamshaIdentityRole>, RamshaRoleStore>();

        // context.Services.TryAddScoped<RamshaUserStore>();


        // context.Services.TryAddScoped(typeof(IUserStore<RamshaIdentityUser>), provider => provider.GetService(typeof(RamshaUserStore)));

        // context.Services.TryAddScoped<RamshaRoleStore>();
        // context.Services.TryAddScoped(typeof(IRoleStore<RamshaIdentityRole>), provider => provider.GetService(typeof(RamshaRoleStore)));

        context.Services.AddIdentityCore<RamshaIdentityUser>(options =>
       {
           options.Password.RequireDigit = false;
           options.Password.RequireNonAlphanumeric = false;
           options.Password.RequireUppercase = false;
           options.Password.RequiredLength = 4;
       })
           .AddRoles<RamshaIdentityRole>()
           .AddSignInManager()
           .AddDefaultTokenProviders()
          .AddClaimsPrincipalFactory<RamshaUserClaimsPrincipalFactory>()
          .AddRamshaIdentity();

        context.Services
                .AddAuthentication(o =>
                {
                    o.DefaultScheme = IdentityConstants.ApplicationScheme;
                    o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                })
                .AddIdentityCookies();

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

public class BasicAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public BasicAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock)
    { }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
            return Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));

        var authHeader = Request.Headers["Authorization"].ToString();
        if (authHeader != "Bearer testtoken")
            return Task.FromResult(AuthenticateResult.Fail("Invalid Token"));

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "123"),
            new Claim(ClaimTypes.Name, "demo-user")
        };

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
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