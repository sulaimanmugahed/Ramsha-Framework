using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha.Identity.Contracts;

public class RamshaIdentityUserDto
{
    public string Id { get; set; }
    public virtual string UserName { get; set; }
    public virtual string? Email { get; set; }
    public virtual bool EmailConfirmed { get; set; }
    public virtual string? PhoneNumber { get; set; }
    public virtual bool PhoneNumberConfirmed { get; set; }
}
