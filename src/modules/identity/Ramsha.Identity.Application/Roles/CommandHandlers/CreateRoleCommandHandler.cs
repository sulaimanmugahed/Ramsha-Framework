using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Ramsha.Identity.Contracts;
using Ramsha.Identity.Domain;
using Ramsha.LocalMessaging.Abstractions;

namespace Ramsha.Identity.Application;

public class CreateRoleCommandHandler<TRole, TId, TCreateRoleDto>(RoleManager<TRole> roleManager) : CommandHandler<CreateRoleCommand<TId, TCreateRoleDto>, TId>
where TId : IEquatable<TId>
where TCreateRoleDto : CreateRamshaIdentityRoleDto
where TRole : RamshaIdentityRole<TId>, new()
{
    public override async Task<TId> HandleAsync(CreateRoleCommand<TId, TCreateRoleDto> message, CancellationToken cancellationToken = default)
    {
        var newRole = Map(message.CreateDto);
        var result = await roleManager.CreateAsync(newRole);
        if (!result.Succeeded)
        {
            throw new Exception(result.Errors.First().Description);
        }
        return newRole.Id;
    }

    public virtual TRole Map(TCreateRoleDto createDto)
    {
        return new TRole() { Id = CreateRoleId(), Name = createDto.Name };
    }

    public virtual TId CreateRoleId()
    {
        object id = Guid.NewGuid();

        if (typeof(TId) == typeof(Guid))
            return (TId)id;
        if (typeof(TId) == typeof(string))
            return (TId)(object)id.ToString();

        return default;
    }


}


