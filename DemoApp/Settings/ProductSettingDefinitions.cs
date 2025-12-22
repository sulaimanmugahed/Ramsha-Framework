using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha.Settings;

namespace DemoApp.Settings;

public class ProductDiscountSettings
{
    public int Discount { get; set; }
    public bool HasDiscount { get; set; }
}

public class ProductSettingNames
{
    public const string GroupName = "ProductsSettings";
    public const string DiscountSettings = "DiscountSettings";
}

public class ProductSettingDefinitions : ISettingDefinitionProvider
{
    public void Define(ISettingDefinitionContext context)
    {
        context.Group(ProductSettingNames.GroupName, group =>
        {
            group.Setting(ProductSettingNames.DiscountSettings, new ProductDiscountSettings
            {
                Discount = 20,
                HasDiscount = true
            });
        });
    }
}
