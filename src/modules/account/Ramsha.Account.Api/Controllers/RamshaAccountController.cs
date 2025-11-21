using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ramsha.Account.Contracts;
using Ramsha.AspNetCore.Mvc;

namespace Ramsha.Account.Api;

[GenericControllerName("account")]
public class RamshaAccountController<TRegisterDto>(IRamshaAccountService<TRegisterDto> accountService)
: RamshaApiController
where TRegisterDto : RamshaRegisterDto, new()
{
    [HttpPost("register")]
    public async Task<RamshaResult<string>> Register(TRegisterDto registerDto)
    => await UnitOfWork(() => accountService.RegisterAsync(registerDto));
}
