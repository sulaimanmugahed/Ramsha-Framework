using CleanWebApiTemplate.Contracts;
using Ramsha;
using Ramsha.LocalMessaging;
using Ramsha.LocalMessaging.Abstractions;

namespace CleanWebApiTemplate.Application;

public class GetHelloWorldQueryHandler : RamshaQueryHandler<GetHelloWorldQuery>
{
    public override async Task<IRamshaResult> HandleAsync(GetHelloWorldQuery message, CancellationToken cancellationToken = default)
    {
        return Success("Hello World !");
    }
}
