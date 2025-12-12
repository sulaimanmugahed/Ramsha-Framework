using CleanWebApiTemplate.Contracts;
using Ramsha;
using Ramsha.LocalMessaging.Abstractions;

namespace CleanWebApiTemplate.Application;

public class GetHelloWorldQueryHandler : QueryHandler<GetHelloWorldQuery, RamshaResult<string>>
{
    public override async Task<RamshaResult<string>> HandleAsync(GetHelloWorldQuery message, CancellationToken cancellationToken = default)
    {
        return "Hello World !";
    }
}
