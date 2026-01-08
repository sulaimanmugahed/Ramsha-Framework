using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Ramsha.AspNetCore.Security.Claims;
using Ramsha.Common.Domain;
using Ramsha.Security.Claims;
using Ramsha.UnitOfWork.Abstractions;

namespace Ramsha.AspNetCore;

public class AspNetCoreModule : RamshaModule
{
    public override void Register(RegisterContext context)
    {
        base.Register(context);
        context.DependsOn<CommonDomainModule>();
    }

    public override void BuildServices(BuildServicesContext context)
    {
        base.BuildServices(context);
        context.Services.AddHttpContextAccessor();
        context.Services.AddSingleton<IPrincipalAccessor, HttpPrincipalAccessor>();
        context.Services.TryAddSingleton<IAppPipeline<IApplicationBuilder>, AppPipeline<IApplicationBuilder>>();
        context.Services.AddProblemDetails();
        context.Services.AddExceptionHandler<RamshaGlobalExceptionHandler>();

        context.Services.Configure<RamshaHooksOptions>(options =>
        {
            options.InitHookContributors.Add<AspNetCoreInitHookContributor>();
        });
    }
}






