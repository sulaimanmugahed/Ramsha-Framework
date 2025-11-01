
using Ramsha.Domain;

namespace DemoApp.Entities;

public class Category : Entity<Guid>
{
    public string Name { get; set; }
    public Category(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public List<Product> Products { get; set; } = [];

    public void AddProduct(Product product)
    {
        if (Products.Any(x => x.Id == product.Id))
        {
            return;
        }
        Products.Add(product);
    }
}
