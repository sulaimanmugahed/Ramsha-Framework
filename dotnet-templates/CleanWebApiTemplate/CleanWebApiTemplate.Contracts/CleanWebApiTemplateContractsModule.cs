using CleanWebApiTemplate.Shared;
using Ramsha;

namespace CleanWebApiTemplate.Contracts;

public class CleanWebApiTemplateContractsModule : RamshaModule
{
    public override void Register(RegisterContext context)
    {
        base.Register(context);
        context.DependsOn<CleanWebApiTemplateSharedModule>();
    }
}
