using Microsoft.EntityFrameworkCore;

namespace CleanWebApiTemplate.Persistence;

public static class ModelBuilderExtensions
{
    public static ModelBuilder ConfigureCleanWebApiTemplate(this ModelBuilder builder)
    {
        // builder.Entity<MyEntity>(entity=>
        //{
        // 
        //}) ;

        return builder;
    }
}
