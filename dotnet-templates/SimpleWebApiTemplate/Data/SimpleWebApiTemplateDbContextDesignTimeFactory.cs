
using Ramsha;
using Ramsha.EntityFrameworkCore;

namespace SimpleWebApiTemplate.Data;

public class SimpleWebApiTemplateDbContextDesignTimeFactory
: RamshaDesignTimeDbContext<DefaultStartupModule, SimpleWebApiTemplateDbContext>
{
    protected override IConfigurationRoot BuildConfiguration()
    => ConfigurationHelper.BuildConfiguration();
}
