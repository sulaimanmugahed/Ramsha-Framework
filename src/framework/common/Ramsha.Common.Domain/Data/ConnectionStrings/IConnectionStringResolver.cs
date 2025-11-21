using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Ramsha.Common.Domain;

public interface IConnectionStringResolver
{
    Task<string?> ResolveAsync(string? connectionStringName = null);

}
