using CleanWebApiTemplate.Contracts;
using Ramsha;
using Ramsha.LocalMessaging;

namespace CleanWebApiTemplate.Application;

public class SayHelloCommandHandler : CommandHandler<SayHelloCommand>
{
    public override async Task<IRamshaResult> HandleAsync(SayHelloCommand message, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(message.Name))
        {
            return Invalid(message: "The name can not be Null Or Empty");
        }

        return Success($"Hello {message.Name}");
    }
}
