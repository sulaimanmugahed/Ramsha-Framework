

namespace Ramsha;

public interface IRamshaHostEnvironment
{
    string? EnvironmentName { get; set; }
}
public class RamshaHostEnvironment : IRamshaHostEnvironment
{
    public string? EnvironmentName { get; set; }
}
