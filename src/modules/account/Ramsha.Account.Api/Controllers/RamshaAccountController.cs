using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ramsha.Account.Contracts;
using Ramsha.AspNetCore.Mvc;

namespace Ramsha.Account.Api;

[ControllerName("account")]
public class RamshaAccountController<TRegisterDto>(IRamshaAccountService<TRegisterDto> accountService)
: RamshaApiController
where TRegisterDto : RamshaRegisterDto, new()
{
    [HttpPost("register")]
    public async Task<ActionResult> Register(TRegisterDto registerDto)
    => await UnitOfWork(async () => RamshaResult(await accountService.RegisterAsync(registerDto)));
}
