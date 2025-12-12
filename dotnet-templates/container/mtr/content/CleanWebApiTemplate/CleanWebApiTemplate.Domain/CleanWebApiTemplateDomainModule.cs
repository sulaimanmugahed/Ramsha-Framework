using CleanWebApiTemplate.Shared;
using Ramsha;
using Ramsha.Common.Domain;

namespace CleanWebApiTemplate.Domain;

public class CleanWebApiTemplateDomainModule : RamshaModule
{
    public override void Register(RegisterContext context)
    {
        base.Register(context);

        context
        .DependsOn<CleanWebApiTemplateSharedModule>()
        .DependsOn<CommonDomainModule>();
    }
}
