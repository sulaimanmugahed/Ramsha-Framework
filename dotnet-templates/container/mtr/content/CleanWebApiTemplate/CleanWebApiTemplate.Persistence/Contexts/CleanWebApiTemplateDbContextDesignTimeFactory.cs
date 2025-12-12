using Microsoft.Extensions.Configuration;
using Ramsha;
using Ramsha.EntityFrameworkCore;

namespace CleanWebApiTemplate.Persistence;

public class CleanWebApiTemplateDbContextDesignTimeFactory
: RamshaDesignTimeDbContext<CleanWebApiTemplatePersistenceModule, CleanWebApiTemplateDbContext>
{
    protected override IConfigurationRoot BuildConfiguration()
    => ConfigurationHelper.BuildConfiguration();
}