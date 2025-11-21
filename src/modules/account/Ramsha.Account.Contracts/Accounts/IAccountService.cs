using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha.Account.Contracts;

public interface IRamshaAccountService<TRegisterDto> : IRamshaAccountServiceBase
where TRegisterDto : RamshaRegisterDto, new()
{
    Task<RamshaResult<string>> RegisterAsync(TRegisterDto registerDto);
}


public interface IRamshaAccountServiceBase
{

}