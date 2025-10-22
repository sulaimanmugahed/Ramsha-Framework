using Autofac.Extensions.DependencyInjection;
using DemoApp;

var builder = WebApplication.CreateBuilder(args);

await builder.AddRamshaAsync<AppModule>();

var app = builder.Build();

await app.UseRamshaAsync();

await app.RunAsync();

