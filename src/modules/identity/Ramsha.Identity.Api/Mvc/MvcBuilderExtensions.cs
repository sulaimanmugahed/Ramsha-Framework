using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Ramsha.AspNetCore.Mvc;
using Ramsha.Identity.Contracts;
using Ramsha.Identity.Shared;

namespace Ramsha.Identity.Api;

public static class MvcBuilderExtensions
{
    public static void AddIdentityGenericControllers(this IMvcBuilder builder)
    {
        var typesOptions = builder.Services.ExecutePreparedOptions<RamshaIdentityTypesOptions>();
        var contractsOptions = builder.Services.ExecutePreparedOptions<RamshaIdentityContractsOptions>();
        builder.AddGenericControllers(
            typeof(RamshaIdentityRoleController<,,,>).MakeGenericType(contractsOptions.GetReplacedDtoOrBase<RamshaIdentityRoleDto>(), contractsOptions.GetReplacedDtoOrBase<CreateRamshaIdentityRoleDto>(), contractsOptions.GetReplacedDtoOrBase<UpdateRamshaIdentityRoleDto>(), typesOptions.KeyType),
            typeof(RamshaIdentityUserController<,,,>).MakeGenericType(contractsOptions.GetReplacedDtoOrBase<RamshaIdentityUserDto>(), contractsOptions.GetReplacedDtoOrBase<CreateRamshaIdentityUserDto>(), contractsOptions.GetReplacedDtoOrBase<UpdateRamshaIdentityUserDto>(), typesOptions.KeyType)
            );
    }
}
