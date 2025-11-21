using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoApp.Controllers;
using Ramsha;
using Ramsha.Common.Domain;

namespace DemoApp.Entities;



public record ProductCreatedEvent(Product Product);

public sealed class Product : AggregateRoot<Guid>, IPrice, IEntityCreation
{
    public string Name { get; set; }

    public decimal Price { get; set; }

    public Guid? CategoryId { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime CreationDate { get; set; }

    private Product(Guid id, string name, decimal price) : base(id)
    {
        Name = name;
        Price = price;
    }

    private Product()
    {

    }

    public static Product Create(Guid id, string name, decimal price)
    {
        var product = new Product(id, name, price);
        product.RaiseEvent(new ProductCreatedEvent(product));
        return product;
    }
}
