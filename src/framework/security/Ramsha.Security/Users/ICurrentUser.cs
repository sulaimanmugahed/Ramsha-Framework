using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ramsha.Security.Users;

public interface ICurrentUser
{
    Guid? Id { get; }
    string? Username { get; }
    string? Email { get; }
    Claim? FindClaim(string claimType);
    Claim[] GetClaims(string claimType);
    Claim[] GetAllClaims();
    string[] GetRoles();
    bool IsAuthenticated();
    bool IsInRole(string roleName);
}
