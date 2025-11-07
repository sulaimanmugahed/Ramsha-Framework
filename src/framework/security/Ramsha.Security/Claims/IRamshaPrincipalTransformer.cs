using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Ramsha.Security.Claims;

public interface IRamshaClaimsPrincipalTransformer
{
    Task TransformAsync(RamshaClaimsPrincipalContext context);
}

