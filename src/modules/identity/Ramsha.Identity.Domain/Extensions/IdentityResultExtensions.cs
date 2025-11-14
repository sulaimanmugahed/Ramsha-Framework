using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Ramsha.Core;
using Ramsha.Identity.Core;

namespace Ramsha.Identity.Domain;

public static class IdentityResultExtensions
{
    public static List<RamshaError> MapToRamshaErrors(this IdentityResult result)
    {
        return result.Errors
        .Select(error =>
         RamshaError.Create(RamshaIdentityErrorsCodes.Prefix + error.Code, error.Description)
         )
         .ToList();
    }
}
