using CleanWebApiTemplate.Contracts;
using Ramsha;
using Ramsha.AspNetCore.Mvc;

namespace CleanWebApiTemplate.Api;

public class CleanWebApiTemplateApiModule : RamshaModule
{
    public override void Register(RegisterContext context)
    {
        base.Register(context);

        context
        .DependsOn<CleanWebApiTemplateContractsModule>()
        .DependsOn<AspNetCoreMvcModule>();
    }
}