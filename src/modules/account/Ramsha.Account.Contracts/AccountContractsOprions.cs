using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha.Common.Contracts;

namespace Ramsha.Account.Contracts;

public class RamshaAccountContractsOptions : ContractsOptionsBase
{
    public Type? ReplacedAccountServiceType { get; private set; }

    public RamshaAccountContractsOptions ReplaceAccountService<TService>()
    where TService : IRamshaAccountServiceBase
    {
        ReplacedAccountServiceType = typeof(TService);
        return this;
    }
}

