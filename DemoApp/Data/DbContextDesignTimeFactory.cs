using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Ramsha;

namespace DemoApp.Data;

public class DbContextDesignTimeFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<AppDbContext>();

        var configuration = ConfigurationHelper.BuildConfiguration();

        builder.UseSqlServer(configuration.GetConnectionString("Default"));

        return new AppDbContext(builder.Options);
    }
}
