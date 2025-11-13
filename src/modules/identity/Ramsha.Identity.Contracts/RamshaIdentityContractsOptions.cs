using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha.Identity.Contracts;

public class RamshaIdentityContractsOptions
{
    public Type CreateRoleDtoType { get; private set; } = typeof(CreateRamshaIdentityRoleDto);
    public Type CreateUserDtoType { get; private set; } = typeof(CreateRamshaIdentityUserDto);
    public Type? ReplacedUserServiceType { get; private set; }

    public RamshaIdentityContractsOptions UserDtoTypes(Type createDto)
    {
        CreateUserDtoType = createDto;
        return this;
    }

    public RamshaIdentityContractsOptions ReplaceUserService<T>()
    {
        ReplacedUserServiceType = typeof(T);
        return this;
    }

    public RamshaIdentityContractsOptions RoleDtoTypes(Type createDto)
    {
        CreateRoleDtoType = createDto;
        return this;
    }
}
