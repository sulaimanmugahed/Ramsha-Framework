using Microsoft.Extensions.DependencyInjection;
using Ramsha.Account.Contracts;
using Ramsha.Core.Modularity.Contexts;

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
        context.Configure<IMvcBuilder>(builder =>
       {
           builder.AddAccountGenericControllers();
       });
    }



}
