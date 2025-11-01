

namespace Ramsha.Domain;


public interface IGlobalQueryFilterManager<TFilter>
 where TFilter : class
{
    IDisposable Enable();

    IDisposable Disable();

    bool IsEnabled { get; }
}

public interface IGlobalQueryFilterManager
{
    IDisposable Enable<TFilter>()
       where TFilter : class;

    IDisposable Disable<TFilter>()
       where TFilter : class;

    bool IsEnabled<TFilter>()
       where TFilter : class;

}


