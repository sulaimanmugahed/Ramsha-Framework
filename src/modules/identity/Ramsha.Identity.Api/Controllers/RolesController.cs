using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ramsha.AspNetCore.Mvc;
using Ramsha.Core;
using Ramsha.Domain;
using Ramsha.Identity.Contracts;

namespace Ramsha.Identity.Api;

[GenericControllerName("roles")]
public class RamshaIdentityRoleController<TDto, TCreateDto, TUpdateDto, TId>(IRamshaIdentityRoleService<TDto, TCreateDto, TUpdateDto, TId> roleService) : RamshaApiController
where TId : IEquatable<TId>
where TCreateDto : CreateRamshaIdentityRoleDto
where TUpdateDto : UpdateRamshaIdentityRoleDto
where TDto : RamshaIdentityRoleDto

{

    [HttpGet("{id}")]
    public async Task<RamshaResult<TDto>> Get(TId id)
    {
        if (UnitOfWorkManager.TryBeginReserved(UnitOfWork.UnitOfWork.UnitOfWorkReservationName, new Ramsha.UnitOfWork.Abstractions.UnitOfWorkOptions { IsTransactional = false }))
        {
            return await roleService.Get(id);
        }

        return RamshaResult<TDto>.Failure();
    }

    [HttpGet]
    public async Task<RamshaResult<List<TDto>>> GetList(TId id)
    {
        if (UnitOfWorkManager.TryBeginReserved(UnitOfWork.UnitOfWork.UnitOfWorkReservationName, new Ramsha.UnitOfWork.Abstractions.UnitOfWorkOptions { IsTransactional = false }))
        {
            return await roleService.GetList(id);
        }

        return RamshaResult<List<TDto>>.Failure();
    }

    [HttpPost]
    public async Task<RamshaResult<string>> Create(TCreateDto createDto)
    {
        if (UnitOfWorkManager.TryBeginReserved(UnitOfWork.UnitOfWork.UnitOfWorkReservationName, new Ramsha.UnitOfWork.Abstractions.UnitOfWorkOptions { IsTransactional = false }))
        {
            return await roleService.Create(createDto);
        }

        return RamshaResult<string>.Failure();
    }

    [HttpPut("{id}")]
    public async Task<RamshaResult> Update(TId id, TUpdateDto updateDto)
    {
        if (UnitOfWorkManager.TryBeginReserved(UnitOfWork.UnitOfWork.UnitOfWorkReservationName, new Ramsha.UnitOfWork.Abstractions.UnitOfWorkOptions { IsTransactional = false }))
        {
            return await roleService.Update(id, updateDto);
        }

        return RamshaResult.Failure();
    }

    [HttpDelete("{id}")]
    public async Task<RamshaResult> Delete(TId id)
    {
        if (UnitOfWorkManager.TryBeginReserved(UnitOfWork.UnitOfWork.UnitOfWorkReservationName, new Ramsha.UnitOfWork.Abstractions.UnitOfWorkOptions { IsTransactional = false }))
        {
            return await roleService.Delete(id);
        }

        return RamshaResult.Failure();
    }

    
}
