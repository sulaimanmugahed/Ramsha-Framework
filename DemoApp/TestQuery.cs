
using Ramsha;
using Ramsha.LocalMessaging.Abstractions;

namespace DemoApp;

public class TestQuery : Query<string>
{

}

public class TestQueryHandler(IRamshaService ramshaService) : QueryHandler<TestQuery, string>
{
    public override Task<string> HandleAsync(TestQuery message, CancellationToken cancellationToken = default)
    {
        var result = ramshaService.Get();
        return Task.FromResult(result);
    }
}
