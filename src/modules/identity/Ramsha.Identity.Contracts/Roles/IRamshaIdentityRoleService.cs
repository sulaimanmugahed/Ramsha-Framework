using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha.Core;

namespace Ramsha.Identity.Contracts;

public interface IRamshaIdentityRoleService<TDto, TCreateDto, TUpdateDto, TId> : IRamshaIdentityRoleServiceBase
    where TId : IEquatable<TId>
    where TCreateDto : CreateRamshaIdentityRoleDto
    where TUpdateDto : UpdateRamshaIdentityRoleDto
    where TDto : RamshaIdentityRoleDto
{
    Task<RamshaResult<string>> Create(TCreateDto createDto);
    Task<RamshaResult> Update(TId id, TUpdateDto updateDto);
    Task<RamshaResult> Delete(TId id);
    Task<RamshaResult<TDto>> Get(TId id);
    Task<RamshaResult<List<TDto>>> GetList(TId id);
}

public interface IRamshaIdentityRoleServiceBase;
