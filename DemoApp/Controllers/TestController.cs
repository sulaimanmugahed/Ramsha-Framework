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

public class TestController(IQueryMediator queryMediator) : RamshaApiController
{
    [HttpGet]
    public async Task<string> Get()
    {
        return await queryMediator.QueryAsync(new TestQuery());
    }
}
