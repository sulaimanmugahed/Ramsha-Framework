using Autofac.Extensions.DependencyInjection;
using DemoApp;
using DemoModule;
using LiteBus.Extensions.Microsoft.DependencyInjection;
using LiteBus.Queries;
using LiteBus.Queries.Abstractions;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ramsha;
using Ramsha.AspNetCore;
using Ramsha.AspNetCore.Mvc;
using Ramsha.Autofac;

using Ramsha.LocalMessaging;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseRamshaServiceProvider();


builder.Services.AddServiceProviderHook<PropertyInitializerServiceProviderHook>(ServiceLifetime.Scoped);

await builder.AddRamshaAppAsync(module =>
{
    module
    .OnCreating(options =>
    {
        options
        .DependsOn<DemoModuleModule>()
        .DependsOn<AspNetCoreMvcModule>()
        .DependsOn<LocalMessagingModule>()
        .DependsOn<AutofacModule>();
    })
    .OnConfigureAsync(async context =>
    {

        builder.Services.AddScoped<IRamshaService, RamshaService>();
        builder.Services.AddScoped<ITestService, TestService>();
        context.Services.AddOpenApi();
        context.Services.AddLiteBus(options =>
      {

          options.AddQueryModule(builder =>
          {

              builder.RegisterFromAssembly(typeof(TestQuery).Assembly);
          });

      });
    })
    .OnInitAsync(async context =>
    {
        var logger = context.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("******************** Application Initialized **********************************");
    });
}, options =>
{
});





var app = builder.Build();

await app.UseRamshaAppAsync();


app.MapGet("get", (ITestService service) =>
{
    return service.Get();
});

app.MapControllers();

await app.RunAsync();

