using Ramsha.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

await builder.AddRamshaAsync(module =>
{
    module
    .Register(ctx =>
    {
        ctx.DependsOn<AspNetCoreMvcModule>();
    });
});

var app = builder.Build();

await app.UseRamshaAsync();

app.MapGet("/ping", () =>
{
    return Results.Ok("Pang !!");
});

await app.RunAsync();
