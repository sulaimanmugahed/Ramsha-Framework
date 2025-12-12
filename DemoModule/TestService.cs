using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha;

namespace DemoModule;

public interface ITestService
{
    string Get();
}
public class TestService : ITestService, IHasPropertyInjection
{
    public string Get()
    {
        return "this is test";
    }
}
