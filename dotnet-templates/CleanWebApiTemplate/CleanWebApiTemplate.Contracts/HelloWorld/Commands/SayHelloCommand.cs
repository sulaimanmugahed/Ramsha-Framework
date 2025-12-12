using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha;
using Ramsha.LocalMessaging.Abstractions;

namespace CleanWebApiTemplate.Contracts;

public class SayHelloCommand : Command<RamshaResult<string>>
{
    public string? Name { get; set; }
}
