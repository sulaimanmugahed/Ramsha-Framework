
using Ramsha.Common.Domain;

namespace DemoApp.Entities;

public class Category : Entity<Guid>, IEntityCreation
{
    public string Name { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime CreationDate { get; set; }
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
