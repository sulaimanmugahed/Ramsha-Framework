using DemoApp;
using DemoModule;

var builder = WebApplication.CreateBuilder(args);

await builder.AddAppAsync<AppModule>();


builder.Services.AddOpenApi();


var app = builder.Build();

await app.InitAppAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("get", (ITestService service) =>
{
    return service.Get();
});

app.Run();

