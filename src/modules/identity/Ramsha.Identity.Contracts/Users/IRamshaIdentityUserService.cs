

using Ramsha.Core;

namespace Ramsha.Identity.Contracts;

public interface IRamshaIdentityUserService<TCreateDto, TUpdateDto, TId>
    where TCreateDto : CreateRamshaIdentityUserDto
{
    Task<RamshaResult<string>> Create(TCreateDto createDto);
    Task<RamshaResult> Update(TUpdateDto updateDto);
    Task<RamshaResult> Delete(TId id);
}
