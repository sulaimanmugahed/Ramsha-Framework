using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoModule;

public interface ITestService
{
    string Get();
}
public class TestService : ITestService
{
    public string Get()
    {
        return "hello";
    }
}
