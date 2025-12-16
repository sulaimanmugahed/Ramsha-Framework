using Cocona;
using Microsoft.Extensions.DependencyInjection;
using Ramsha.Cli.Core;

var builder = CoconaApp.CreateBuilder();

builder.Services.RegisterInfrastructure();


var app = builder.Build();

app.AddSubCommand("add", options =>
{
    options.AddCommand("controller", async (
         ITemplateGenerator generator,
    [Argument] string name,
    [Option('n')] string? @namespace = null,
    [Option('o')] string outputPath = ".",
    [Option('v')] bool mvc = false
    ) =>
    {
        try
        {
            await generator.GenerateAsync(
         new TemplateModel(EmbeddedTemplates.Controller.Name)
          .WithName(name)
          .WithNamespace(@namespace)
          .WithOutput(outputPath)
          .WithParam("mvc", mvc)
    );
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }

    }).WithAliases("contr", "con")
    .WithDescription("Adds a new controller"); ;
});

app.AddSubCommand("new", options =>
{
    options.AddCommand("api", async (
        IDotnetTemplateGenerator generator,
        [Argument] string name,
        [Option('o')] string output = ".",
        [Option('c')] bool clean = false,
        [Option('f')] bool force = false,
        [Option('d', Description = "Include database support")] bool useDatabase = false,
        [Option(Description = "Database provider: mssql, postgres, lite")] string dbProvider = "mssql"
     ) =>
    {
        try
        {
            await generator.GenerateAsync(
               new DotnetTemplateModel(clean ? DotnetTemplates.CleanWebApi.Name : DotnetTemplates.WebApi.Name)
               .WithName(name)
               .WithOutput(Path.Combine(output, name))
               .WithForce(force)
                .WithParam("useDatabase", useDatabase)
                .WithParam("dbProvider", dbProvider, condition: useDatabase)
               );
            Console.WriteLine("Project created successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }).WithDescription("Creates a new Web API project using Ramsha framework");
});

app.Run();