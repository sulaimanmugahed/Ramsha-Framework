using SimpleAppDemo;

var builder = WebApplication.CreateBuilder(args);

await builder.AddRamshaAsync<SimpleAppModule>();

var app = builder.Build();

await app.UseRamshaAsync();
app.Run();
