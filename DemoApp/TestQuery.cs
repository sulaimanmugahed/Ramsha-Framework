using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha;
using Ramsha;
using Ramsha.LocalMessaging.Abstractions;

namespace DemoApp;

public class TestQuery : Query<string>
{

}

public class TestQueryHandler : QueryHandler<TestQuery, string>
{
    [Injectable]
    protected IRamshaService RamshaService { get; set; }
    public override Task<string> HandleAsync(TestQuery message, CancellationToken cancellationToken = default)
    {
        var result = RamshaService.Get();
        return Task.FromResult(result);
    }
}
