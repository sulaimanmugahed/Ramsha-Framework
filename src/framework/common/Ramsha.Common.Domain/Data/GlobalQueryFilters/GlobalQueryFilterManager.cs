using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Ramsha.Common.Domain;

public class GlobalQueryFilterManager : IGlobalQueryFilterManager
{
    private readonly ConcurrentDictionary<Type, object> _filters;

    private readonly IServiceProvider _serviceProvider;

    public GlobalQueryFilterManager(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _filters = new ConcurrentDictionary<Type, object>();
    }

    public IDisposable Enable<TFilter>()
       where TFilter : class
    {
        return GetFilter<TFilter>().Enable();
    }

    public IDisposable Disable<TFilter>()
       where TFilter : class
    {
        return GetFilter<TFilter>().Disable();
    }

    public bool IsEnabled<TFilter>()
       where TFilter : class
    {
        return GetFilter<TFilter>().IsEnabled;
    }

    private IGlobalQueryFilterManager<TFilter> GetFilter<TFilter>()
       where TFilter : class
    {
        return (_filters.GetOrAdd(
            typeof(TFilter),
            factory => _serviceProvider.GetRequiredService<IGlobalQueryFilterManager<TFilter>>()
        ) as IGlobalQueryFilterManager<TFilter>)!;
    }
}


public class GlobalQueryFilterManager<TFilter> : IGlobalQueryFilterManager<TFilter>
   where TFilter : class
{
    public bool IsEnabled
    {
        get
        {
            EnsureInitialized();
            return _filter.Value!.IsEnabled;
        }
    }


    private readonly AsyncLocal<GlobalQueryFilterState> _filter;
    private readonly GlobalQueryFilterOptions _options;

    public GlobalQueryFilterManager(IOptions<GlobalQueryFilterOptions> options)
    {
        _filter = new AsyncLocal<GlobalQueryFilterState>();
        _options = options.Value;
    }

    public IDisposable Enable()
    {
        if (IsEnabled)
        {
            return NullDisposable.Instance;
        }

        _filter.Value!.IsEnabled = true;

        return new OnDispose(() => Disable());
    }

    public IDisposable Disable()
    {
        if (!IsEnabled)
        {
            return NullDisposable.Instance;
        }

        _filter.Value!.IsEnabled = false;

        return new OnDispose(() => Enable());
    }

    private void EnsureInitialized()
    {
        if (_filter.Value != null)
        {
            return;
        }

        if (_options.DefaultStates != null && _options.DefaultStates.TryGetValue(typeof(TFilter), out var state))
        {
            _filter.Value = state.Clone();
        }
        else
        {
            _filter.Value = new GlobalQueryFilterState(true);
        }
    }
}

