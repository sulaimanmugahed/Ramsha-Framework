using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha.Identity.Contracts;
using Ramsha.LocalMessaging.Abstractions;

namespace Ramsha.Identity.Application.Users.CommandHandlers;

public class CreateUserCommandHandler<TUser, TId, TCreateDto> : CommandHandler<CreateUserCommand<TUser, TId, TCreateDto>, TId>
where TId : IEquatable<TId>
where TCreateDto : CreateRamshaIdentityUserDto
{
    public override Task<TId> HandleAsync(CreateUserCommand<TUser, TId, TCreateDto> message, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
