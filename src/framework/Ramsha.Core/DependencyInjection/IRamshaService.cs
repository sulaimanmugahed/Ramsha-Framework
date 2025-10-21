using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha;

public class RamshaService : IRamshaService
{
    public string Get() => "This is Ramsha Service";
}

public interface IRamshaService
{
    string Get();
}

