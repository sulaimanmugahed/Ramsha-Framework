using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ramsha.Security.Claims;

public class RamshaClaimsPrincipalFactoryOptions
{
    public ITypeList<IRamshaClaimsPrincipalTransformer> Transformers { get; }
    public Dictionary<string, List<string>> ClaimsMap { get; set; }

    public RamshaClaimsPrincipalFactoryOptions()
    {
        Transformers = new TypeList<IRamshaClaimsPrincipalTransformer>();

        ClaimsMap = new Dictionary<string, List<string>>()
        {
            { RamshaClaimsTypes.Username, new List<string> { "preferred_username", "unique_name", ClaimTypes.Name }},
            { RamshaClaimsTypes.Name, new List<string> { "given_name", ClaimTypes.GivenName }},
            { RamshaClaimsTypes.SurName, new List<string> { "family_name", ClaimTypes.Surname }},
            { RamshaClaimsTypes.Role, new List<string> { "role", "roles", ClaimTypes.Role }},
            { RamshaClaimsTypes.Email, new List<string> { "email", ClaimTypes.Email }},
        };

    }
}

