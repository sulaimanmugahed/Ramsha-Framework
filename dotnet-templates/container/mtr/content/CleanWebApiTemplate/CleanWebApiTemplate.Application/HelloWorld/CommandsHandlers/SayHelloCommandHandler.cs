using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanWebApiTemplate.Contracts;
using Ramsha;
using Ramsha.Core;
using Ramsha.LocalMessaging.Abstractions;

namespace CleanWebApiTemplate.Application;

public class SayHelloCommandHandler : CommandHandler<SayHelloCommand, RamshaResult<string>>
{
    public override async Task<RamshaResult<string>> HandleAsync(SayHelloCommand message, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(message.Name))
        {
            return RamshaError.Create(
                RamshaErrorsCodes.ValidationErrorCode,
                "The name most have value",
                nameof(message.Name)
                );
        }

        return $"Hello {message.Name}";
    }
}
