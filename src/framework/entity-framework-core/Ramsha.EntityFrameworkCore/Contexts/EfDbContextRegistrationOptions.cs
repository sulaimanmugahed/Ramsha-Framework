using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Ramsha.Common.Domain;

namespace Ramsha.EntityFrameworkCore;

public interface IEfDbContextRegistrationOptionsBuilder : IDbContextRegistrationOptionsBaseBuilder
{
}

public class EfDbContextRegistrationOptions : DbContextRegistrationOptionsBase, IEfDbContextRegistrationOptionsBuilder
{
    public EfDbContextRegistrationOptions(Type originalDbContextType, IServiceCollection services)
        : base(originalDbContextType, services)
    {
    }
}