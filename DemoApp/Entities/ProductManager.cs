using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoApp.Data;
using Ramsha;
using Ramsha.Common.Domain;

namespace DemoApp.Entities;

public class ProductManager(IProductRepository repository)
: RamshaDomainManager
{


    public async Task<IRamshaResult> Delete(Guid id)
    {
        return await UnitOfWork<IRamshaResult>(async () =>
        {
            var product = await repository.FindAsync(id);
            if (product is null)
            {
                return NotFound();
            }

            await repository.DeleteAsync(id);
            return Success();
        });
    }
    public async Task<RamshaResult<string>> Create(string name, decimal price)
    {
        return await UnitOfWork<RamshaResult<string>>(async () =>
        {
            var product = await repository.AddAsync(Product.Create(Guid.NewGuid(), name, price));
            if (product is null)
            {
                return Invalid();
            }

            return product.Id.ToString();
        });
    }
}
