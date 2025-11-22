using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ramsha;

public class BuildServicesContext(IServiceCollection services)
{
    public IServiceCollection Services { get; } = services;
    public IConfiguration Configuration => _config ??= Services.GetConfiguration();
    private IConfiguration? _config;

}