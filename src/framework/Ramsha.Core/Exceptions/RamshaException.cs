using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha;

public class RamshaException: Exception
{
    public RamshaException()
    {

    }

    public RamshaException(string? message)
        : base(message)
    {

    }

    public RamshaException(string? message, Exception? inner)
        : base(message, inner)
    {

    }
}


