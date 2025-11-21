using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha.Account.Contracts;
using Ramsha.Identity.Contracts;

namespace DemoApp;

public class AppRegisterDto : RamshaRegisterDto
{
    public string Profile { get; set; }

}
public class CreateAppUserDto : CreateRamshaIdentityUserDto
{
    public string Profile { get; set; }
}

public class UpdateAppUserDto : UpdateRamshaIdentityUserDto
{
    public string? Profile { get; set; }
}