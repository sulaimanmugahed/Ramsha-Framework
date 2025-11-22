using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha.Core.Modularity.Contexts;

namespace Ramsha;

public class DefaultStartupModuleBuilder
{
    private readonly DefaultStartupModule _module;

    public DefaultStartupModuleBuilder()
    {
        _module = new DefaultStartupModule();
    }


    public DefaultStartupModuleBuilder OnCreating(Action<PrepareContext> context)
    {
        _module.AddModuleBuilderAction(context);
        return this;
    }

    public DefaultStartupModuleBuilder OnConfigureAsync(Func<BuildServicesContext, Task> configure)
    {
        _module.AddConfigureAsyncAction(configure);
        return this;
    }

    public DefaultStartupModuleBuilder OnInitAsync(Func<InitContext, Task> init)
    {
        _module.AddInitAsyncAction(init);
        return this;
    }

    public DefaultStartupModuleBuilder OnShutdownAsync(Func<ShutdownContext, Task> shutdown)
    {
        _module.AddShutdownAsyncAction(shutdown);
        return this;
    }


    public DefaultStartupModuleBuilder OnConfigure(Action<BuildServicesContext> configure)
    {
        _module.AddConfigureAction(configure);
        return this;
    }


    public DefaultStartupModuleBuilder OnInit(Action<InitContext> init)
    {
        _module.AddInitAction(init);
        return this;
    }


    public DefaultStartupModuleBuilder OnShutdown(Action<ShutdownContext> shutdown)
    {
        _module.AddShutdownAction(shutdown);
        return this;
    }


    internal DefaultStartupModule Build() => _module;
}

public sealed class DefaultStartupModule : RamshaModule
{
    private readonly List<Action<BuildServicesContext>> _configureActions = new();
    private readonly List<Action<InitContext>> _initActions = new();
    private readonly List<Action<ShutdownContext>> _shutdownActions = new();

    private readonly List<Func<BuildServicesContext, Task>> _asyncConfigureActions = new();
    private readonly List<Func<InitContext, Task>> _asyncInitActions = new();
    private readonly List<Func<ShutdownContext, Task>> _asyncShutdownActions = new();

    private readonly List<Action<PrepareContext>> _modulePrepareActions = new();
    private readonly List<Func<PrepareContext, Task>> _asyncModulePrepareActions = new();



    internal void AddModuleBuilderAction(Action<PrepareContext> action) => _modulePrepareActions.Add(action);
    internal void AddConfigureAction(Action<BuildServicesContext> action) => _configureActions.Add(action);
    internal void AddInitAction(Action<InitContext> action) => _initActions.Add(action);
    internal void AddShutdownAction(Action<ShutdownContext> action) => _shutdownActions.Add(action);

    internal void AddConfigureAsyncAction(Func<BuildServicesContext, Task> action) => _asyncConfigureActions.Add(action);
    internal void AddInitAsyncAction(Func<InitContext, Task> action) => _asyncInitActions.Add(action);
    internal void AddShutdownAsyncAction(Func<ShutdownContext, Task> action) => _asyncShutdownActions.Add(action);

    public override async Task BuildServicesAsync(BuildServicesContext context)
    {
        foreach (var action in _asyncConfigureActions)
        {
            await action(context);
        }
    }

    public override async Task OnInitAsync(InitContext context)
    {
        foreach (var action in _asyncInitActions)
        {
            await action(context);
        }
    }

    public override async Task OnShutdownAsync(ShutdownContext context)
    {
        foreach (var action in _asyncShutdownActions)
        {
            await action(context);
        }
    }





    public override void Prepare(PrepareContext context)
    {
        foreach (var action in _modulePrepareActions)
        {
            action(context);
        }
    }

    public override async Task PrepareAsync(PrepareContext context)
    {
        foreach (var action in _asyncModulePrepareActions)
        {
            await action(context);
        }
    }

    public override void BuildServices(BuildServicesContext context)
    {
        foreach (var action in _configureActions)
            action(context);
    }

    public override void OnInit(InitContext context)
    {
        foreach (var action in _initActions)
            action(context);
    }

    public override void OnShutdown(ShutdownContext context)
    {
        foreach (var action in _shutdownActions)
            action(context);
    }
}
