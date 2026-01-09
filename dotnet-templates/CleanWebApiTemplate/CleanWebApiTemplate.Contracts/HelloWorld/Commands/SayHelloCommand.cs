using Ramsha.LocalMessaging.Abstractions;

namespace CleanWebApiTemplate.Contracts;

public class SayHelloCommand : IRamshaCommand
{
    public string? Name { get; set; }
}
