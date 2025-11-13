using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha.LocalMessaging.Abstractions;

namespace Ramsha.Identity.Contracts;

public class CreateUserCommand<TUser, TId, TCreateDto>:Command<TId>
where TCreateDto : CreateRamshaIdentityUserDto
{
    public TCreateDto CreateDto { get; set; }
}
