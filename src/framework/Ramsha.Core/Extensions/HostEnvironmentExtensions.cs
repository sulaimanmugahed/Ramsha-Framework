
using Microsoft.Extensions.Hosting;

namespace Ramsha;

public static class HostEnvironmentExtensions
{
    public static bool IsDevelopment(this IRamshaHostEnvironment hostEnvironment)
    {
        return hostEnvironment.IsEnvironment(Environments.Development);
    }

    public static bool IsStaging(this IRamshaHostEnvironment hostEnvironment)
    {

        return hostEnvironment.IsEnvironment(Environments.Staging);
    }

    public static bool IsProduction(this IRamshaHostEnvironment hostEnvironment)
    {
        return hostEnvironment.IsEnvironment(Environments.Production);
    }

    public static bool IsEnvironment(this IRamshaHostEnvironment hostEnvironment, string environmentName)
    {

        return string.Equals(
            hostEnvironment.EnvironmentName,
            environmentName,
            StringComparison.OrdinalIgnoreCase);
    }


}
