using Ramsha.AspNetCore.Mvc;
#if (useDatabase)
using SimpleWebApiTemplate.Data;
#endif
#if (useSqlServer)
using Ramsha.EntityFrameworkCore.SqlServer;
#endif

var builder = WebApplication.CreateBuilder(args);

await builder.AddRamshaAsync(module =>
{
    module
    .Register(ctx =>
    {
        ctx.DependsOn<AspNetCoreMvcModule>();
#if (useSqlServer)
        ctx.DependsOn<EntityFrameworkCoreSqlServerModule>();
#endif
    })
    .BuildServices(ctx =>
    {
#if (useDatabase)
        ctx.Services.AddRamshaDbContext<SimpleWebApiTemplateDbContext>();
#endif
    });

});

var app = builder.Build();

await app.UseRamshaAsync();

app.MapGet("/ping", () =>
{
    return Results.Ok("Pang !!");
});

await app.RunAsync();
