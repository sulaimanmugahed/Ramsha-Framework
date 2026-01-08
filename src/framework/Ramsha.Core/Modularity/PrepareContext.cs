
using Microsoft.Extensions.DependencyInjection;

namespace Ramsha;

public class PrepareContext(IServiceCollection services)
{
  public PrepareContext PrepareOptions<TOptions>(Action<TOptions> optionsAction)
    where TOptions : class
  {
    services.PrepareOptions(optionsAction);
    return this;
  }

  public TOptions? PrepareOptions<TOptions>()
    where TOptions : class, new()
  {
    return services.ExecutePreparedOptions<TOptions>();
  }
}
