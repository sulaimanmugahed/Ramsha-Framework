
using Ramsha;
using Ramsha.EntityFrameworkCore;

namespace SimpleWebApiTemplate.Data;

public class SimpleWebApiTemplateDbContextDesignTimeFactory
: RamshaDesignTimeDbContext<SimpleWebApiTemplateModule, SimpleWebApiTemplateDbContext>
{
    protected override IConfigurationRoot BuildConfiguration()
    => ConfigurationHelper.BuildConfiguration();
}
