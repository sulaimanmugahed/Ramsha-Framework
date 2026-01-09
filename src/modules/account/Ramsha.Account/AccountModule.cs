using Ramsha.Account.Api;
using Ramsha.Account.Application;

namespace Ramsha.Account;

public class AccountModule : RamshaModule
{
    public override void Register(RegisterContext context)
    {
        base.Register(context);
        context
        .DependsOn<AccountApplicationModule>()
        .DependsOn<AccountApiModule>();
    }
}
