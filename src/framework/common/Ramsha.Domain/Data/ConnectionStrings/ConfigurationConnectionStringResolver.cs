using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Ramsha.Domain;

public class DefaultConnectionStringResolver(IOptions<ConnectionStringsOptions> options) : IConnectionStringResolver
{
    public Task<string?> ResolveAsync(string? connectionStringName = null)
    {
        return Task.FromResult(options.Value.Get(connectionStringName));
    }
}
