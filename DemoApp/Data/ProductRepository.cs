using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoApp.Entities;
using Ramsha.EntityFrameworkCore;

namespace DemoApp.Data;

public class ProductRepository(IDbContextProvider<AppDbContext> dbContextProvider
) : EFCoreRepository<AppDbContext, Product, Guid>(dbContextProvider), IProductRepository
{

}
