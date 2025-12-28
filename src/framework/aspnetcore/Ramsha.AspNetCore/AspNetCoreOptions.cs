using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha.AspNetCore;

public class AspNetCoreOptions
{
    public bool HttpsRedirection { get; set; } = true;
    public bool ExceptionHandler { get; set; } = true;

}
