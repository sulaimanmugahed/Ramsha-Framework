using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Ramsha.Account.Contracts;
using Ramsha.AspNetCore.Mvc;


namespace Ramsha.Account.Api;

public static class MvcBuilderExtensions
{
    public static void AddAccountGenericControllers(this IMvcBuilder builder)
    {
        var contractsOptions = builder.Services.ExecutePreConfigured<RamshaAccountContractsOptions>();
        builder.AddGenericControllers(
            typeof(RamshaAccountController<>).MakeGenericType(contractsOptions.GetReplacedDtoOrBase<RamshaRegisterDto>())
            );
    }
}
