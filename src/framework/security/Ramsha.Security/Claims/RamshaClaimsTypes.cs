using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ramsha.Security.Claims;

public static class RamshaClaimsTypes
{
    public static string Username { get; set; } = ClaimTypes.Name;
    public static string Name { get; set; } = ClaimTypes.GivenName;
    public static string SurName { get; set; } = ClaimTypes.Surname;
    public static string UserId { get; set; } = ClaimTypes.NameIdentifier;
    public static string Role { get; set; } = ClaimTypes.Role;
    public static string Email { get; set; } = ClaimTypes.Email;
}

