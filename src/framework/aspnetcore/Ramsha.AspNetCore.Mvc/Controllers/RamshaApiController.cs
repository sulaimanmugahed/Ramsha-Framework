using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Ramsha.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public abstract class RamshaApiController : RamshaControllerBase
{

}
