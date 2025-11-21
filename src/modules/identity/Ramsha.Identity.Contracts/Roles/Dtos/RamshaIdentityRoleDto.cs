using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha.Identity.Contracts;

public class RamshaIdentityRoleDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public bool IsBase { get; set; }

}
