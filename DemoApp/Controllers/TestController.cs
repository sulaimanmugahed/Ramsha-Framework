using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoModule;
using LiteBus.Queries.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Ramsha;
using Ramsha.AspNetCore.Mvc;

namespace DemoApp.Controllers;

public class TestController : RamshaApiController
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await Mediator.Send(new TestQuery()));
    }
}
