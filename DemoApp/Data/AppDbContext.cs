using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoApp.Entities;
using Microsoft.EntityFrameworkCore;
using Ramsha.Domain;
using Ramsha.EntityFrameworkCore;

namespace DemoApp;

[ConnectionString("MainDb")]
public class AppDbContext(DbContextOptions<AppDbContext> options)
: RamshaDbContext<AppDbContext>(options)
{
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Product>(b =>
        {
            b.HasKey(p => p.Id);
            b.Property(p => p.Name).IsRequired().HasMaxLength(200);
            b.Property(p => p.Price).HasColumnType("decimal(18,2)");
        });
    }
}
