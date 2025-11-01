using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoApp.Entities;
using Microsoft.AspNetCore.Mvc;
using Ramsha.AspNetCore.Mvc;
using Ramsha.Domain;
using Ramsha.UnitOfWork;

namespace DemoApp.Controllers;

public class CategoriesController(IRepository<Product> productRepo, IRepository<Category, Guid> categoryRepo) : RamshaApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        List<Category> categories = [];
        if (UowManager.TryBeginReserved(UnitOfWork.UnitOfWorkReservationName, new UnitOfWorkOptions()))
        {
            using var _ = GlobalQueryFilterManager.Disable<IPrice>();
            categories = await categoryRepo.GetListAsync(includes: x => x.Products);
        }
        return Ok(categories);
    }
    [HttpPost]
    public async Task<IActionResult> Create(string name)
    {
        if (UowManager.TryBeginReserved(UnitOfWork.UnitOfWorkReservationName, new UnitOfWorkOptions()))
        {
            await categoryRepo.CreateAsync(new Category(Guid.NewGuid(), name));
        }
        return Ok();
    }
    [HttpPost(nameof(SeedAllProductsToCategory))]
    public async Task<IActionResult> SeedAllProductsToCategory(Guid categoryId)
    {
        if (UowManager.TryBeginReserved(UnitOfWork.UnitOfWorkReservationName, new UnitOfWorkOptions()))
        {
            using var _ = GlobalQueryFilterManager.Disable<IPrice>();
            var products = await productRepo.GetListAsync();

            var category = await categoryRepo.FindAsync(categoryId, x => x.Products);
            if (category is null)
            {
                return NotFound("no category Found");
            }

            foreach (var product in products)
            {
                category.AddProduct(product);
            }

            if (UowManager.Current is not null)
            {
                await UowManager.Current.SaveChangesAsync();
            }
        }
        return Ok();

    }
}
