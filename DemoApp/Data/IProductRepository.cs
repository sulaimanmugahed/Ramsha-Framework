using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoApp.Entities;
using Ramsha.Common.Domain;

namespace DemoApp.Data;

public interface IProductRepository : IRepository<Product, Guid>
{

}
