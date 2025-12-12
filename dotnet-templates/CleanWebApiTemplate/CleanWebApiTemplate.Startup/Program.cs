using CleanWebApiTemplate.Startup;

var builder = WebApplication.CreateBuilder(args);

await builder.AddRamshaAsync<CleanWebApiTemplateStartupModule>();

var app = builder.Build();

await app.UseRamshaAsync();

await app.RunAsync();


