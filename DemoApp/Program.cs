using DemoApp;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRamsha(ramsha =>
{
    ramsha.AddModule<AppModule>();
});

var app = builder.Build();

app.UseRamsha();
app.Run();


