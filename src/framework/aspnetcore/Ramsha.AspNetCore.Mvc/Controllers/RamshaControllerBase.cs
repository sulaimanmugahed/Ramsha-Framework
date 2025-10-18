using Microsoft.AspNetCore.Mvc;

namespace Ramsha.AspNetCore.Mvc;


public abstract class RamshaControllerBase : ControllerBase
{
    public IRamshaServiceProvider ServiceProvider = default!;
}
