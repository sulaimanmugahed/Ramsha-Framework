using CleanWebApiTemplate.Api;
using CleanWebApiTemplate.Application;
using Ramsha;

namespace CleanWebApiTemplate.Startup;

public class CleanWebApiTemplateStartupModule : RamshaModule
{
    public override void Register(RegisterContext context)
    {
        base.Register(context);

        context
        .DependsOn<CleanWebApiTemplateApplicationModule>()
        .DependsOn<CleanWebApiTemplateApiModule>();
    }
}
