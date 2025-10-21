using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiteBus.Commands.Abstractions;

namespace Ramsha.LocalMessaging.Abstractions;

public class Command : IRamshaCommand
{

}

public class Command<TResult> : IRamshaCommand<TResult>
{

}

