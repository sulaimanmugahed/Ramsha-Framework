using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Ramsha.Common.Domain;

public class DefaultConnectionStringResolver(IOptions<ConnectionStringsOptions> options) : IConnectionStringResolver
{
    public Task<string?> ResolveAsync(string? connectionStringName = null)
    {
        var con = options.Value.Get(connectionStringName ?? "Default");
        return Task.FromResult<string?>(con);
    }
}
