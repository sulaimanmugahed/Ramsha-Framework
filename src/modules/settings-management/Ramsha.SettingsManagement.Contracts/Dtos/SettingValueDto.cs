using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha.SettingsManagement.Contracts;

public record SettingValueDto(string Name, object? Value);

