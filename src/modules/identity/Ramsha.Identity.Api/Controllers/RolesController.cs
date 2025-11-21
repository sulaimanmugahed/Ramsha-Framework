using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ramsha.AspNetCore.Mvc;
using Ramsha.Core;
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
    => await UnitOfWork(() => roleService.Get(id));

    [HttpGet]
    public async Task<RamshaResult<List<TDto>>> GetList(TId id)
  => await UnitOfWork(() => roleService.GetList(id));

    [HttpPost]
    public async Task<RamshaResult<string>> Create(TCreateDto createDto)
    => await UnitOfWork(() => roleService.Create(createDto));

    [HttpPut("{id}")]
    public async Task<RamshaResult> Update(TId id, TUpdateDto updateDto)
   => await UnitOfWork(() => roleService.Update(id, updateDto));


    [HttpDelete("{id}")]
    public async Task<RamshaResult> Delete(TId id)
    => await UnitOfWork(() => roleService.Delete(id));



}
