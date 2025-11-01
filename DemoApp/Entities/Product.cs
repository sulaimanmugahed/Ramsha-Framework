using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoApp.Controllers;
using Ramsha;
using Ramsha.Domain;

namespace DemoApp.Entities;

public class Product : Entity<Guid>, IPrice
{
    public string Name { get; set; }

    public decimal Price { get; set; }

    public Guid? CategoryId { get; set; }

    public Product(Guid id, string name, decimal price) : base(id)
    {
        Name = name;
        Price = price;
    }
}
