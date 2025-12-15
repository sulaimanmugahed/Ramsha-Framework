using CleanWebApiTemplate.Api;
using CleanWebApiTemplate.Application;
#if (useDatabase)
using CleanWebApiTemplate.Persistence;
#endif
using Ramsha;

namespace CleanWebApiTemplate.Startup;

public class CleanWebApiTemplateStartupModule : RamshaModule
{
    public override void Register(RegisterContext context)
    {
        base.Register(context);

        context
        .DependsOn<CleanWebApiTemplateApplicationModule>()
#if (useDatabase)
        .DependsOn<CleanWebApiTemplatePersistenceModule>()
#endif
        .DependsOn<CleanWebApiTemplateApiModule>();
    }
}
