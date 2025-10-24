using Microsoft.AspNetCore.Mvc;

namespace Ramsha.AspNetCore.Mvc;


public abstract class RamshaControllerBase : ControllerBase
{
    [Injectable]
    public IServiceProvider ServiceProvider { get; set; } = default!;
}
