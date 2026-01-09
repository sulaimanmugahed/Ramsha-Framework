using CleanWebApiTemplate.Contracts;
using CleanWebApiTemplate.Domain;
using Ramsha;
using Ramsha.Common.Application;
using Ramsha.LocalMessaging.Abstractions;

namespace CleanWebApiTemplate.Application;

public class CleanWebApiTemplateApplicationModule : RamshaModule
{
    public override void Register(RegisterContext context)
    {
        base.Register(context);

        context
        .DependsOn<CleanWebApiTemplateContractsModule>()
        .DependsOn<CleanWebApiTemplateDomainModule>()
        .DependsOn<CommonApplicationModule>();
    }

    public override void Prepare(PrepareContext context)
    {
        base.Prepare(context);

        context.PrepareOptions<LocalMessagingOptions>(options =>
        {
            options.AddMessagesFromAssembly<CleanWebApiTemplateApplicationModule>();
        });
    }
}