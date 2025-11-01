using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha.Domain;

public class GlobalQueryFilterOptions
{
    public Dictionary<Type, GlobalQueryFilterState> DefaultStates { get; }

    public GlobalQueryFilterOptions()
    {
        DefaultStates = new Dictionary<Type, GlobalQueryFilterState>();
    }
}
