using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha.Settings;

namespace DemoApp.Settings;

public class ProductSettingDefinitions : ISettingDefinitionProvider
{
    public void Define(ISettingDefinitionContext context)
    {
        context.Group("ProductsSettings", group =>
        {
            group.Setting("DiscountSettings", new ProductDiscountSettings
            {
                Discount = 20,
                HasDiscount = true
            });
        });
    }
}
