

using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Ramsha;
using Ramsha.Common.Domain;
using Ramsha.EntityFrameworkCore;
using Ramsha.EntityFrameworkCore.SqlServer;
using Ramsha.Identity;
using Ramsha.Identity.Persistence;
using Ramsha.Settings;
using SimpleAppDemo;

var builder = WebApplication.CreateBuilder(args);


//var ramsha = builder.AddRamshaApp<AppModule>();

var ramsha = builder.Services.AddRamsha(ramsha =>
{
    ramsha
    .AddModule<AppModule>()
    .AddIdentity()
    .AddEFSqlServerProvider();
});

builder.Services.AddRamshaDbContext<AppDbContext>();

var app = builder.Build();

app.UseRamsha();

app.MapGet("modules", () =>
{
    return ModuleLogHelper.BuildModuleTreeDiagram(ramsha.Modules);

    // return JsonSerializer.Serialize(ramsha.Modules.Select(x => new
    // {
    //     module = x.Type.Name,
    //     dependencies = x.Dependencies.Select(x => x.Type.Name)
    // }).ToList(), new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.IgnoreCycles });

});

app.Run();

public class AppDbContext(DbContextOptions<AppDbContext> options)
: RamshaEFDbContext<AppDbContext>(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ConfigureIdentity();
    }
}

public class AppModule : RamshaModule
{
    public override void Register(RegisterContext context)
    {
        base.Register(context);
        context.DependsOn<IdentityModule>();
        context.DependsOn<EntityFrameworkCoreSqlServerModule>();
    }

    public override void BuildServices(BuildServicesContext context)
    {
        base.BuildServices(context);
        context.Services.AddRamshaDbContext<AppDbContext>();
    }
}
