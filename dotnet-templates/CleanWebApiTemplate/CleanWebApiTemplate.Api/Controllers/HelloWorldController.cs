using CleanWebApiTemplate.Contracts;
using Microsoft.AspNetCore.Mvc;
using Ramsha;
using Ramsha.AspNetCore.Mvc;

namespace CleanWebApiTemplate.Api;

public class HelloWorldController : RamshaApiController
{
  [HttpGet]
  public async Task<RamshaResult<string>> GetHelloWorld()
    => await Mediator.Send(new GetHelloWorldQuery());

  [HttpPost]
  public async Task<RamshaResult<string>> SayHello(SayHelloCommand command)
    => await Mediator.Send(command);
}
