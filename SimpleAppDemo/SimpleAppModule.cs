using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha;
using Ramsha.AspNetCore.Mvc;
using Ramsha.Common.Domain;
using Ramsha.Core.Modularity.Contexts;
using Ramsha.UnitOfWork;

namespace SimpleAppDemo;

public class SimpleAppModule : RamshaModule
{
    public override void Register(RegisterContext context)
    {
        base.Register(context);

        context
        .DependsOn<UnitOfWorkModule>()
        .DependsOn<AspNetCoreMvcModule>();
    }

    public override void Prepare(PrepareContext context)
    {
        base.Prepare(context);
        context.Configure<TestDomainOptions>(options =>
{
    options.Name = "simple";
});
    }

    public override void BuildServices(BuildServicesContext context)
    {
        var testOptions = context.Services.ExecutePreparedOptions<TestDomainOptions>();

        base.BuildServices(context);


    }


}
