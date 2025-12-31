
using DemoApp.Data;
using DemoApp.Entities;
using DemoModule;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Ramsha;
using Ramsha.AspNetCore.Mvc;
using Ramsha.Common.Domain;
using Ramsha.UnitOfWork;
using Ramsha.UnitOfWork.Abstractions;

namespace DemoApp.Controllers;




public class ProductsController(ProductManager productManager, IGlobalQueryFilterManager dataFilter, IProductRepository repository, IServiceScopeFactory serviceScopeFactory, IOptionsMonitor<TestSetting> options) : RamshaApiController
{
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        return RamshaResult(await productManager.Delete(id));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product?>> Get(Guid id)
    {
        var enabled = GlobalQueryFilterManager.IsEnabled<ISoftDelete>();

        return await UnitOfWork(() => repository.FindAsync(id));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await UnitOfWork(() => repository.GetListAsync());
        return Ok(products);
    }

    [HttpPost]
    public async Task<ActionResult<string>> Create(string name, decimal price)
    {
        return RamshaResult(await productManager.Create(name, price));
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
