using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha;
using Ramsha;

namespace DemoModule;

public interface ITestService
{
    string Get();
}
public class TestService : ITestService, IHasPropertyInjection
{
    [Injectable]
    public IRamshaService RamshaService { get; set; }
    public string Get()
    {
        return RamshaService.Get();
    }
}
