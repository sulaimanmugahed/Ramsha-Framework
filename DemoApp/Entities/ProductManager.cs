using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoApp.Data;
using Ramsha;
using Ramsha.Common.Domain;
using Ramsha.Core;
using Ramsha.UnitOfWork.Abstractions;

namespace DemoApp.Entities;

public class ProductManager(IProductRepository repository)
: RamshaDomainManager
{


    public async Task<RamshaResult> Delete(Guid id)
    {
        return await UnitOfWork(async () =>
        {
            var product = await repository.FindAsync(id);
            if (product is null)
            {
                return RamshaError.Create(RamshaErrorsCodes.EntityNotFoundErrorCode);
            }

            await repository.DeleteAsync(id);
            return RamshaResult.Ok();
        });
    }
    public async Task<RamshaResult<string>> Create(string name, decimal price)
    {
        return await UnitOfWork(async () =>
        {
            var product = await repository.AddAsync(Product.Create(Guid.NewGuid(), name, price));
            if (product is null)
            {
                return RamshaResult<string>.Failure();
            }

            return product.Id.ToString();
        });
    }
}
