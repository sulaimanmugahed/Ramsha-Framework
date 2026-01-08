using Microsoft.Extensions.DependencyInjection;
using Ramsha.Account.Contracts;

namespace Ramsha.Account.Api;

public class AccountApiModule : RamshaModule
{
    public override void Register(RegisterContext context)
    {
        base.Register(context);
        context.DependsOn<AccountContractsModule>();
    }
    public override void Prepare(PrepareContext context)
    {
        base.Prepare(context);
        context.PrepareOptions<IMvcBuilder>(builder =>
       {
           builder.AddAccountGenericControllers();
       });
    }



}
