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
  public async Task<ActionResult<TDto>> Get(TId id)
  => RamshaResult(await UnitOfWork(() => roleService.Get(id)));

  [HttpGet]
  public async Task<ActionResult<List<TDto>>> GetList(TId id)
=> RamshaResult(await UnitOfWork(() => roleService.GetList(id)));

  [HttpPost]
  public async Task<ActionResult<string>> Create(TCreateDto createDto)
  => RamshaResult(await UnitOfWork(() => roleService.Create(createDto)));

  [HttpPut("{id}")]
  public async Task<IActionResult> Update(TId id, TUpdateDto updateDto)
 => RamshaResult(await UnitOfWork(() => roleService.Update(id, updateDto)));


  [HttpDelete("{id}")]
  public async Task<IActionResult> Delete(TId id)
  => RamshaResult(await UnitOfWork(() => roleService.Delete(id)));



}
