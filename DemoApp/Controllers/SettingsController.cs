using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoApp.Settings;
using Microsoft.AspNetCore.Mvc;
using Ramsha.AspNetCore.Mvc;
using Ramsha.Settings;

namespace DemoApp.Controllers;

public class AppSettingsController(ISettingResolver settingResolver) : RamshaApiController
{
    [HttpGet(nameof(GetProductDiscountSettings))]
    public async Task<ProductDiscountSettings?> GetProductDiscountSettings()
    {
        return await settingResolver.ResolveAsync<ProductDiscountSettings>("DiscountSettings");
    }
}
