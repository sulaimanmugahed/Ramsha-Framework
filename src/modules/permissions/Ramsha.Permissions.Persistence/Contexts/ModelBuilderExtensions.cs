using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ramsha.Permissions.Domain;

namespace Ramsha.Permissions.Persistence;

public static class ModelBuilderExtensions
{
    public static ModelBuilder ConfigurePermissions(this ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.ToTable("Permissions");
            entity.HasKey(x => x.Id);
        });

        return modelBuilder;
    }
}
