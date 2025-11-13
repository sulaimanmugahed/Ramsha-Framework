using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ramsha.AspNetCore.Mvc;
using Ramsha.Core;
using Ramsha.Identity.Contracts;

namespace Ramsha.Identity.Api;

[GenericControllerName("Users")]
public class UsersController<TCreateDto, TUpdateDto, TId>(IRamshaIdentityUserService<TCreateDto, TUpdateDto, TId> userService) : RamshaApiController
where TCreateDto : CreateRamshaIdentityUserDto
{
    [HttpPost]
    public async Task<RamshaResult<string>> Create(TCreateDto createDto)
    {
        if (UnitOfWorkManager.TryBeginReserved(UnitOfWork.UnitOfWork.UnitOfWorkReservationName, new UnitOfWork.Abstractions.UnitOfWorkOptions()))
        {
            return await userService.Create(createDto);
        }
        return RamshaResult<string>.Failure();
    }

}
