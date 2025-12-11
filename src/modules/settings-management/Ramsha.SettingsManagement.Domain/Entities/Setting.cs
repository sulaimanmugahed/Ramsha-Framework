using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha.Common.Domain;

namespace Ramsha.SettingsManagement.Domain;

public class Setting : Entity<Guid>
{
    public string Name { get; set; }
    public string? Value { get; set; }
    public string ProviderName { get; set; }
    public string? ProviderKey { get; set; }

}
