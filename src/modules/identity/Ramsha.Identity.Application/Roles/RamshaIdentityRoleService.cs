using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha.Common.Application;
using Ramsha.Core;
using Ramsha.Identity.Contracts;
using Ramsha.Identity.Domain;

namespace Ramsha.Identity.Application;

public class RamshaIdentityRoleService<TRole, TId, TUserRole, TRoleClaim, TDto, TCreateDto, TUpdateDto>(RamshaIdentityRoleManager<TRole, TId, TUserRole, TRoleClaim> manager, IIdentityRoleRepository<TRole, TId> repository) : RamshaService, IRamshaIdentityRoleService<TDto, TCreateDto, TUpdateDto, TId>
where TRole : RamshaIdentityRole<TId, TUserRole, TRoleClaim>, new()
    where TId : IEquatable<TId>
    where TUserRole : RamshaIdentityUserRole<TId>, new()
    where TRoleClaim : RamshaIdentityRoleClaim<TId>, new()
    where TCreateDto : CreateRamshaIdentityRoleDto, new()
    where TUpdateDto : UpdateRamshaIdentityRoleDto, new()
    where TDto : RamshaIdentityRoleDto, new()

{

    public virtual async Task<RamshaResult<TDto>> Get(TId id)
    {
        var role = await manager.FindByIdAsync(id.ToString());
        if (role is null)
        {
            return NotFound(message: "no role found with this id");
        }

        return ToDto(role);
    }
    public virtual async Task<RamshaResult<string>> Create(TCreateDto createDto)
    {
        var newRole = MapCreate(createDto);
        var result = await manager.CreateAsync(newRole);
        if (!result.Succeeded)
        {
            return result.MapToRamshaError();
        }

        return newRole.Id.ToString()!;
    }

    protected virtual TDto ToDto(TRole role)
    {
        return new TDto()
        {
            Id = role.Id.ToString(),
            Name = role.Name,
            IsBase = role.IsBase
        };
    }

    protected virtual TRole MapCreate(TCreateDto createDto)
    {
        return new TRole()
        {
            Id = GenerateId(),
            Name = createDto.Name,
            IsBase = createDto.IsBase
        };
    }

    protected virtual TId GenerateId()
    {
        object id = Guid.NewGuid();

        if (typeof(TId) == typeof(Guid))
            return (TId)id;
        if (typeof(TId) == typeof(string))
            return (TId)(object)id.ToString();

        return default;
    }
    protected virtual TRole MapUpdate(TRole user, TUpdateDto updateDto)
    {
        user.Name = updateDto.Name;
        return user;
    }

    public async Task<IRamshaResult> Delete(TId id)
    {
        var role = await manager.FindByIdAsync(id.ToString());
        if (role is null)
        {
            return NotFound(message: "no role found with this id");
        }

        var result = await manager.DeleteAsync(role);

        if (!result.Succeeded)
        {
            return result.MapToRamshaError();
        }
        return Success();

    }

    public async Task<IRamshaResult> Update(TId id, TUpdateDto updateDto)
    {
        var role = await manager.FindByIdAsync(id.ToString());
        if (role is null)
        {
            return NotFound(message: "no role found with this id");
        }

        MapUpdate(role, updateDto);
        var result = await manager.UpdateAsync(role);
        if (!result.Succeeded)
        {
            return result.MapToRamshaError();
        }
        return Success();
    }

    public async Task<RamshaResult<List<TDto>>> GetList(TId id)
    {
        var roles = await repository.GetListAsync();
        return roles.Select(ToDto).ToList();
    }
}
