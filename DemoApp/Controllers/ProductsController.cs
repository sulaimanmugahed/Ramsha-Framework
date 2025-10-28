
using DemoApp.Entities;
using Microsoft.AspNetCore.Mvc;
using Ramsha.AspNetCore.Mvc;
using Ramsha.Domain;
using Ramsha.UnitOfWork;

namespace DemoApp.Controllers;

public class ProductsController(IRepository<Product, Guid> repository, IUnitOfWorkManager uowManager, IServiceScopeFactory serviceScopeFactory) : RamshaApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // using var scope = serviceScopeFactory.CreateScope();
        // var manager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
        List<Product> products = null;
        if (uowManager.TryBeginReserved(UnitOfWork.UnitOfWorkReservationName, new UnitOfWorkOptions()))
        {
            products = await repository.GetListAsync();
        }
        return Ok(products);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string name, decimal price)
    {
        if (uowManager.TryBeginReserved(UnitOfWork.UnitOfWorkReservationName, new UnitOfWorkOptions { IsTransactional = true }))
        {
            await repository.CreateAsync(new Product(Guid.NewGuid(), name, price));
            if (uowManager.Current is not null)
            {
                await uowManager.Current.SaveChangesAsync();
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
