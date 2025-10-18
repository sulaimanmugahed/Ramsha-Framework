using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoModule;
using Microsoft.AspNetCore.Mvc;
using Ramsha;
using Ramsha.AspNetCore.Mvc;

namespace DemoApp.Controllers;

public class TestController : RamshaApiController
{
    [HttpGet]
    public string Get()
    {
        var testService = ServiceProvider.GetRequiredService<ITestService>();
        return testService.Get();
    }
}
