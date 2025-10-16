using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha;

public interface IModuleContainer
{
    IReadOnlyList<IModuleDescriptor> Modules { get; }
}