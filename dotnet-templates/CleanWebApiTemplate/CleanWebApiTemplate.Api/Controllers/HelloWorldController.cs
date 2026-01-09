using CleanWebApiTemplate.Contracts;
using Microsoft.AspNetCore.Mvc;
using Ramsha;
using Ramsha.AspNetCore.Mvc;

namespace CleanWebApiTemplate.Api;

public class HelloWorldController : RamshaApiController
{
  [HttpGet]
  public async Task<IActionResult> GetHelloWorld()
    => await Query(new GetHelloWorldQuery());

  [HttpPost]
  public async Task<IActionResult> SayHello(SayHelloCommand command)
    => await Command(command);
}
