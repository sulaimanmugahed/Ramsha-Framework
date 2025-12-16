using MyApp.Startup;

var builder = WebApplication.CreateBuilder(args);

await builder.AddRamshaAsync<MyAppStartupModule>();

var app = builder.Build();

await app.UseRamshaAsync();

await app.RunAsync();


