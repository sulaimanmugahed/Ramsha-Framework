using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Ramsha;
using Ramsha.EntityFrameworkCore;

namespace DemoApp.Data;

public class DbContextDesignTimeFactory : RamshaDesignTimeDbContext<AppModule, AppDbContext>
{

    protected override IConfigurationRoot BuildConfiguration()
    {
        return ConfigurationHelper.BuildConfiguration();
    }
}
