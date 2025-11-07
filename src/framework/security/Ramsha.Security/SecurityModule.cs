using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Ramsha.Security.Claims;
using Ramsha.Security.Users;

namespace Ramsha.Security;

public class SecurityModule : RamshaModule
{
    public override void OnConfiguring(ConfigureContext context)
    {
        base.OnConfiguring(context);

        context.Services.AddSingleton<IPrincipalAccessor, ThreadPrincipalAccessor>();
        context.Services.AddTransient<IRamshaClaimsPrincipalFactory, RamshaClaimsPrincipalFactory>();
        context.Services.AddScoped<ICurrentUser, CurrentUser>();
        context.Services.AutoRegisterPrincipalTransformers();
    }


}


