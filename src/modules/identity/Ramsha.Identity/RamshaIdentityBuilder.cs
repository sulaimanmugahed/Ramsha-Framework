
using Microsoft.Extensions.DependencyInjection;
using Ramsha.Common.Domain;
using Ramsha.EntityFrameworkCore;
using Ramsha.Identity.Domain;

namespace Ramsha.Identity;

public class RamshaIdentityBuilder
{
    public IServiceCollection Services { get; }

    public RamshaIdentityBuilder(IServiceCollection services)
    {
        Services = services;
    }
}
