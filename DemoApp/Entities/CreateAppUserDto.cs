using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha.Identity.Contracts;

namespace DemoApp.Entities;

public class CreateAppUserDto : CreateRamshaIdentityUserDto
{
    public string Profile { get; set; }
}

public class UpdateAppUserDto : UpdateRamshaIdentityUserDto
{
    public string? Profile { get; set; }
}