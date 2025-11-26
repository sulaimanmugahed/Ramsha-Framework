using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Options;
using Ramsha.Common.Domain;
using Ramsha.EntityFrameworkCore;

namespace DemoApp.Entities;

public class TestSetting
{
    public bool Value { get; set; }
}


public class PriceFilterProvider : EFGlobalQueryFilterProvider<AppDbContext, IPrice>
{
    protected override Expression<Func<IPrice, bool>> CreateFilterExpression(AppDbContext dbContext)
    {
        return x => x.Price >= 1000;
    }

}


