using Microsoft.Extensions.DependencyInjection;
using Ramsha.Account.Contracts;

namespace Ramsha.Account.Api;

public class AccountApiModule : RamshaModule
{
    public override void OnCreating(ModuleBuilder moduleBuilder)
    {
        base.OnCreating(moduleBuilder);
        moduleBuilder.DependsOn<AccountContractsModule>();

        moduleBuilder.OnCreatingConfigure<IMvcBuilder>(builder =>
     {
         builder.AddAccountGenericControllers();
     });
    }


}
