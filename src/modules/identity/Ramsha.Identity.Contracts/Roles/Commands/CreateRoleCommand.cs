using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha.LocalMessaging.Abstractions;

namespace Ramsha.Identity.Contracts;

public class CreateRoleCommand<TId, TCreateRoleDto> : Command<TId>
where TCreateRoleDto : CreateRamshaIdentityRoleDto
{
    public TCreateRoleDto CreateDto { get; set; }
}


