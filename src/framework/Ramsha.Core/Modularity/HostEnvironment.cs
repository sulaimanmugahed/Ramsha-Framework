using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha;

public interface IRamshaHostEnvironment
{
    string? EnvironmentName { get; set; }
}
public class RamshaHostEnvironment : IRamshaHostEnvironment
{
    public string? EnvironmentName { get; set; }
}
