using SimpleWebApiTemplate;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRamsha(r => r.AddModule<SimpleWebApiTemplateModule>());

var app = builder.Build();

app.UseRamsha();

app.MapGet("/ping", () =>
{
    return Results.Ok("Pang !!");
});

app.Run();
