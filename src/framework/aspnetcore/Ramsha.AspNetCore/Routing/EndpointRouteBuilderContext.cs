using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;

namespace Microsoft.AspNetCore.Routing;

public class EndpointRouteBuilderContext
{
    public IEndpointRouteBuilder Endpoints { get; }

    public IServiceProvider ScopeServiceProvider { get; }

    public EndpointRouteBuilderContext(
        IEndpointRouteBuilder endpoints,
        IServiceProvider scopeServiceProvider)
    {
        Endpoints = endpoints;
        ScopeServiceProvider = scopeServiceProvider;
    }
}

public class RamshaEndpointRouterOptions
{
    public List<Action<EndpointRouteBuilderContext>> EndpointConfigureActions { get; }

    public RamshaEndpointRouterOptions()
    {
        EndpointConfigureActions = new List<Action<EndpointRouteBuilderContext>>();
    }
}
