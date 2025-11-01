using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Ramsha;
using Ramsha.Domain;
using Ramsha.EntityFrameworkCore;

namespace DemoApp;

[ConnectionString("MainDb")]
public class AppDbContext(DbContextOptions<AppDbContext> options)
: RamshaEFDbContext<AppDbContext>(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }


    public bool IsPriceFilterEnabled => GlobalDataFilterManager.IsEnabled<IPrice>();

    public string Name => "";


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Category>(e =>
        {
            e.HasKey(c => c.Id);
        });
        modelBuilder.Entity<Product>(b =>
        {
            b.HasKey(p => p.Id);
            b.Property(p => p.Name).IsRequired().HasMaxLength(200);
            b.Property(p => p.Price).HasColumnType("decimal(18,2)");

            b.HasOne<Category>()
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.CategoryId);
        });
    }
}
