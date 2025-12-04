using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha.Common.Domain;

namespace Ramsha.Permissions.Domain;

public sealed class Permission : Entity<Guid>
{
    public string Name { get; set; }
    public string ProviderName { get; set; }
    public string? ProviderKey { get; set; }
}
