using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Ramsha.Cli.Core;

namespace Ramsha.Cli.Infrastructure;

public class DotnetTemplateGenerator(ITemplateRegistry<DotnetTemplateInfo> registry)
    : BaseTemplateGenerator<DotnetTemplateModel, DotnetTemplateInfo>(registry), IDotnetTemplateGenerator

{
    public override async Task GenerateAsync(DotnetTemplateModel model)
    {
        var template = GetTemplateInfo(model.TemplateName);

        await EnsureTemplateAvailableAsync(template);
        await GenerateFromTemplateAsync(model, template);
    }

    private async Task EnsureTemplateAvailableAsync(DotnetTemplateInfo template)
    {
        Console.WriteLine($"Checking template availability: {template.Name}");
        if (!await IsTemplateAvailableAsync(template))
        {
            var package = template.PackageName
                ?? throw new Exception($"No package found for template '{template.Name}'");

            if (!await InstallTemplateAsync(package))
            {
                throw new Exception($"Failed to install template package: {package}");
            }
        }
    }

    private async Task GenerateFromTemplateAsync(DotnetTemplateModel model, DotnetTemplateInfo template)
    {
        var args = BuildDotnetNewCommandArgs(model, template);
        await ExecuteDotnetCommandAsync(args);

    }

    private async Task ExecuteDotnetCommandAsync(string commandArgs)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = commandArgs,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        Console.WriteLine($"Executing: dotnet {commandArgs}");

        using var process = new Process { StartInfo = psi };
        try
        {
            process.Start();


            var outputTask = process.StandardOutput.ReadToEndAsync();
            var errorTask = process.StandardError.ReadToEndAsync();

            await process.WaitForExitAsync();

            var output = await outputTask;
            var error = await errorTask;

            if (!string.IsNullOrWhiteSpace(output))
            {
                Console.WriteLine(output);
            }

            if (process.ExitCode != 0)
            {
                var errorMessage = $"Command failed (exit code {process.ExitCode})";
                if (!string.IsNullOrWhiteSpace(error))
                {
                    errorMessage += $":\n{error}";
                }
                throw new Exception(errorMessage);
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to execute dotnet command: {commandArgs}", ex);
        }
    }

    protected virtual string BuildDotnetNewCommandArgs(DotnetTemplateModel model, DotnetTemplateInfo template)
    {
        var args = new List<string>
            {
                "new", template.ShortName,
            };



        foreach (var param in model.GetParameters())
        {
            AddParameterToArgs(args, param.Key, param.Value);
        }



        return string.Join(" ", args.Select(EscapeArgument));
    }

    private void AddParameterToArgs(List<string> args, string name, object value)
    {
        if (value is bool b)
        {
            if (b)
            {
                args.Add($"--{name}");
            }

            return;
        }
        args.Add($"--{name}");
        args.Add(ConvertValueToString(value));
    }

    private string ConvertValueToString(object value)
    {
        return value switch
        {
            null => throw new ArgumentNullException(nameof(value)),
            string s => s,
            bool b => b.ToString().ToLowerInvariant(),
            IFormattable f => f.ToString(null, CultureInfo.InvariantCulture),
            _ => value.ToString() ?? string.Empty
        };
    }

    private string EscapeArgument(string arg) =>
        arg.Contains(' ') ? $"\"{arg}\"" : arg;





    protected virtual Dictionary<string, string> GetTemplateParameters(DotnetTemplateModel model)
        => [];

    public static async Task<bool> IsTemplateAvailableAsync(DotnetTemplateInfo template)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"new list {template.ShortName}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        };

        using var process = Process.Start(psi)!;
        await process.WaitForExitAsync();

        var output = await process.StandardOutput.ReadToEndAsync();
        return output.Contains(template.ShortName);
    }



    public static async Task<bool> InstallTemplateAsync(string packageName)
    {
        Console.WriteLine($"Installing template package: {packageName}");

        var psi = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"new install {packageName}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        };

        using var process = Process.Start(psi)!;
        await process.WaitForExitAsync();

        if (process.ExitCode != 0)
        {
            var error = await process.StandardError.ReadToEndAsync();
            Console.Error.WriteLine(error);
            return false;
        }

        return true;
    }

}