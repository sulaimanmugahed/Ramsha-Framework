using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Ramsha.AspNetCore.Mvc;
using Ramsha.Identity.Contracts;
using Ramsha.Identity.Core.Options;

namespace Ramsha.Identity.Api;

public static class MvcBuilderExtensions
{
    public static void AddIdentityGenericControllers(this IMvcBuilder builder)
    {
        var typesOptions = builder.Services.ExecutePreConfigured<RamshaIdentityTypesOptions>();
        var contractsOptions = builder.Services.ExecutePreConfigured<RamshaIdentityContractsOptions>();
        builder.AddGenericControllers(
            typeof(RolesController<,,>).MakeGenericType(typesOptions.RoleType, typesOptions.KeyType, contractsOptions.CreateRoleDtoType),
            typeof(UsersController<,,>).MakeGenericType(contractsOptions.CreateUserDtoType, contractsOptions.CreateUserDtoType, typesOptions.KeyType)
            );
    }
}
