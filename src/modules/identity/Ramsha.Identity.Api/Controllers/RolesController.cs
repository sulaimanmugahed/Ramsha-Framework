using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ramsha.AspNetCore.Mvc;
using Ramsha.Domain;
using Ramsha.Identity.Contracts;

namespace Ramsha.Identity.Api;

[GenericControllerName("Roles")]
public class RolesController<TRole, TId, TCreateDto> : RamshaApiController
where TCreateDto : CreateRamshaIdentityRoleDto
{
    [HttpPost]
    public async Task<IActionResult> CreateAsync(TCreateDto createDto)
    {
        if (UnitOfWorkManager.TryBeginReserved(UnitOfWork.UnitOfWork.UnitOfWorkReservationName, new Ramsha.UnitOfWork.Abstractions.UnitOfWorkOptions { IsTransactional = false }))
        {
            var result = await Mediator.Send(new CreateRoleCommand<TId, TCreateDto> { CreateDto = createDto });

            return Ok(result);
        }

        return BadRequest();
    }
}
