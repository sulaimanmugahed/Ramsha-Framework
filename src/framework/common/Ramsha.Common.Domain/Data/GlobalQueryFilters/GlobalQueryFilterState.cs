

namespace Ramsha.Common.Domain;

public class GlobalQueryFilterState
{
    public bool IsEnabled { get; set; }

    public GlobalQueryFilterState(bool isEnabled)
    {
        IsEnabled = isEnabled;
    }

    public GlobalQueryFilterState Clone()
    {
        return new GlobalQueryFilterState(IsEnabled);
    }
}
