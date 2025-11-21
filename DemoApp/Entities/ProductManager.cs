using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoApp.Data;
using Ramsha;
using Ramsha.Common.Domain;
using Ramsha.UnitOfWork.Abstractions;

namespace DemoApp.Entities;

public class ProductManager(IProductRepository repository)
: RamshaDomainManager
{
    public async Task<RamshaResult<string>> Create(string name, decimal price)
    {
        return await UnitOfWork(async () =>
        {
            var product = await repository.CreateAsync(Product.Create(Guid.NewGuid(), name, price));
            if (product is null)
            {
                return RamshaResult<string>.Failure();
            }

            return product.Id.ToString();
        });
    }
}
