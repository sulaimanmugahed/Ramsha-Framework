using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha.Identity.Contracts;

public class CreateRamshaIdentityUserDto
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string[]? Roles { get; set; } 

}

