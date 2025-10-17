using DemoApp;
using DemoModule;

var builder = WebApplication.CreateBuilder(args);

await builder.AddRamshaAppAsync(module =>
{
    module
    .OnCreating(options =>
    {
        options.DependsOn<DemoModuleModule>();
    })
    .OnConfigureAsync(async context =>
    {
        context.Services.AddOpenApi();
    })
    .OnInitAsync(async context =>
    {
        var logger = context.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("******************** Application Initialized **********************************");
    });
});


var app = builder.Build();

await app.UseRamshaAppAsync();


app.MapGet("get", (ITestService service) =>
{
    return service.Get();
});

app.Run();

