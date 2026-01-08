using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Ramsha;

public class AppCreationOptions
{
    [NotNull]
    public IServiceCollection Services { get; }

    [NotNull]
    public ConfigurationBuilderOptions Configuration { get; }

    public string? AppName { get; set; }

    public string? Environment { get; set; }

    public AppCreationOptions([NotNull] IServiceCollection services)
    {
        Services = services;
        Configuration = new ConfigurationBuilderOptions();
    }
}

public class ConfigurationBuilderOptions
{
    public Assembly? UserSecretsAssembly { get; set; }


    public string? UserSecretsId { get; set; }

    public string FileName { get; set; } = "appsettings";


    public bool Optional { get; set; } = true;

    public bool ReloadOnChange { get; set; } = true;

    public string? EnvironmentName { get; set; }

    public string? BasePath { get; set; }

    public string? EnvironmentVariablesPrefix { get; set; }

    public string[]? CommandLineArgs { get; set; }
}