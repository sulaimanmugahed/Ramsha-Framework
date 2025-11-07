using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Ramsha.Security.Claims;

public class RamshaClaimsPrincipalFactory : IRamshaClaimsPrincipalFactory //ITransientDependency
{
    public static string AuthenticationType => "Ramsha.App";

    protected IServiceProvider ServiceProvider { get; }
    protected RamshaClaimsPrincipalFactoryOptions Options { get; }

    public RamshaClaimsPrincipalFactory(
        IServiceProvider serviceProvider,
        IOptions<RamshaClaimsPrincipalFactoryOptions> abpClaimOptions)
    {
        ServiceProvider = serviceProvider;
        Options = abpClaimOptions.Value;
    }

    public virtual async Task<ClaimsPrincipal> CreateAsync(ClaimsPrincipal? existsClaimsPrincipal = null)
    {
        return await InternalCreateAsync(Options, existsClaimsPrincipal);
    }

    public virtual async Task<ClaimsPrincipal> InternalCreateAsync(RamshaClaimsPrincipalFactoryOptions options, ClaimsPrincipal? existsClaimsPrincipal = null)
    {
        var claimsPrincipal = existsClaimsPrincipal ?? new ClaimsPrincipal(new ClaimsIdentity(
            AuthenticationType,
            RamshaClaimsTypes.Username,
            RamshaClaimsTypes.Role));

        var context = new RamshaClaimsPrincipalContext(claimsPrincipal, ServiceProvider);


        foreach (var contributorType in options.Transformers)
        {
            var contributor = (IRamshaClaimsPrincipalTransformer)ServiceProvider.GetRequiredService(contributorType);
            await contributor.TransformAsync(context);
        }



        return context.ClaimsPrincipal;
    }
}



