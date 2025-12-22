using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Ramsha.Authorization;

public class AuthorizationModule : RamshaModule
{
    public override void BuildServices(BuildServicesContext context)
    {
        base.BuildServices(context);

        context.Services.AddAuthorizationServices();

        context.Configure<RamshaPermissionOptions>(options =>
        {
            options.ValueResolvers.Add<UserPermissionValueResolver>();
        });
    }


    public override async Task OnInitAsync(InitContext context)
    {
        base.OnInit(context);
        _ = context.ServiceProvider.GetRequiredService<IPermissionDefinitionStore>();
    }
}
