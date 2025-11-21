using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ramsha.Security.Claims;
using Ramsha.Security.Users;

namespace Ramsha.Security;

public class SecurityModule : RamshaModule
{
    public override void OnConfiguring(ConfigureContext context)
    {
        base.OnConfiguring(context);

        context.Services.TryAddSingleton<IPrincipalAccessor, ThreadPrincipalAccessor>();
        context.Services.TryAddTransient<IRamshaClaimsPrincipalFactory, RamshaClaimsPrincipalFactory>();
        context.Services.TryAddScoped<ICurrentUser, CurrentUser>();
        context.Services.AutoRegisterPrincipalTransformers();
    }


}


