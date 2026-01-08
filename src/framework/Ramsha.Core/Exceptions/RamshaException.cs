

namespace Ramsha;

public class RamshaException : Exception
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


