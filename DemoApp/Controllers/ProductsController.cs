
using DemoApp.Data;
using DemoApp.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Ramsha.AspNetCore.Mvc;
using Ramsha.Domain;
using Ramsha.UnitOfWork;
using Ramsha.UnitOfWork.Abstractions;

namespace DemoApp.Controllers;

public interface IFilter
{

}


public class ProductsController(IGlobalQueryFilterManager dataFilter, IProductRepository repository, IServiceScopeFactory serviceScopeFactory, IOptionsMonitor<TestSetting> options) : RamshaApiController
{


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        List<Product> products = null;

        if (UnitOfWorkManager.TryBeginReserved(UnitOfWork.UnitOfWorkReservationName, new UnitOfWorkOptions()))
        {
            if (options.CurrentValue.Value)
            {
                products = await repository.GetListAsync();

            }
            else
            {
                var tes1 = dataFilter.IsEnabled<IPrice>();
                using (dataFilter.Disable<IPrice>())
                {
                    var tes2 = dataFilter.IsEnabled<IPrice>();

                    products = await repository.GetListAsync();
                }
                var test3 = dataFilter.IsEnabled<IPrice>();

            }

        }
        return Ok(products);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string name, decimal price)
    {
        if (UnitOfWorkManager.TryBeginReserved(UnitOfWork.UnitOfWorkReservationName, new UnitOfWorkOptions { IsTransactional = true }))
        {
            await repository.CreateAsync(Product.Create(Guid.NewGuid(), name, price));
            if (UnitOfWorkManager.Current is not null)
            {
                await UnitOfWorkManager.Current.SaveChangesAsync();
                //throw new Exception("test");
            }
        }

        return Ok();
    }


}

// public class ProductsController(IRepository<Product, Guid> repository, IServiceScopeFactory serviceScopeFactory) : RamshaApiController
// {
//     [HttpGet]
//     public async Task<IActionResult> GetAll()
//     {
//         List<Product> products = null;
//         using var scope = serviceScopeFactory.CreateScope();
//         var uowManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();

//         if (uowManager.TryBeginReserved(UnitOfWork.UnitOfWorkReservationName, new UnitOfWorkOptions()))
//         {
//             products = await repository.GetListAsync();
//         }
//         return Ok(products);
//     }

//     [HttpPost]
//     public async Task<IActionResult> Create(string name, decimal price)
//     {
//         using var scope = serviceScopeFactory.CreateScope();
//         var uowManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();

//         if (uowManager.TryBeginReserved(UnitOfWork.UnitOfWorkReservationName, new UnitOfWorkOptions()))
//         {
//             await repository.CreateAsync(new Product(Guid.NewGuid(), name, price));
//             if (uowManager.Current is not null)
//             {
//                 await uowManager.Current.SaveChangesAsync();
//             }
//         }

//         return Ok();
//     }


// }
