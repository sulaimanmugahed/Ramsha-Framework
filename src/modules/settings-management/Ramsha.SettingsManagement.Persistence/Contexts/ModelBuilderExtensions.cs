using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ramsha.SettingsManagement.Domain;

namespace Ramsha.SettingsManagement.Persistence;

public static class ModelBuilderExtensions
{
    public static ModelBuilder ConfigureSettingsManagement(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Setting>(entity =>
        {
            entity.ToTable("Settings");
        });
        return modelBuilder;
    }
}
