using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Ramsha;

public class BootstrapLogEntry
{
    public LogLevel LogLevel { get; set; }
    public EventId EventId { get; set; }
    public object State { get; set; } = null!;
    public Exception? Exception { get; set; }
    public Func<object, Exception?, string> Formatter { get; set; } = null!;
}
