using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Ramsha.Security.Claims;

namespace Ramsha.AspNetCore.Security.Claims;

public class HttpPrincipalAccessor : ThreadPrincipalAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpPrincipalAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override ClaimsPrincipal GetClaimsPrincipal()
    {
        return _httpContextAccessor.HttpContext?.User ?? base.GetClaimsPrincipal();
    }
}
