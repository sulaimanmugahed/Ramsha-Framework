using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha;

public class DefaultStartupModuleBuilder
{
    private readonly DefaultStartupModule _module;

    public DefaultStartupModuleBuilder()
    {
        _module = new DefaultStartupModule();
    }


    public DefaultStartupModuleBuilder OnCreating(Action<ModuleBuilder> moduleBuilder)
    {
        _module.AddModuleBuilderAction(moduleBuilder);
        return this;
    }

    public DefaultStartupModuleBuilder OnConfigureAsync(Func<ConfigureContext, Task> configure)
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


    public DefaultStartupModuleBuilder OnConfigure(Action<ConfigureContext> configure)
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
    private readonly List<Action<ConfigureContext>> _configureActions = new();
    private readonly List<Action<InitContext>> _initActions = new();
    private readonly List<Action<ShutdownContext>> _shutdownActions = new();

    private readonly List<Func<ConfigureContext, Task>> _asyncConfigureActions = new();
    private readonly List<Func<InitContext, Task>> _asyncInitActions = new();
    private readonly List<Func<ShutdownContext, Task>> _asyncShutdownActions = new();

    private readonly List<Action<ModuleBuilder>> _moduleBuilderActions = new();


    internal void AddModuleBuilderAction(Action<ModuleBuilder> action) => _moduleBuilderActions.Add(action);
    internal void AddConfigureAction(Action<ConfigureContext> action) => _configureActions.Add(action);
    internal void AddInitAction(Action<InitContext> action) => _initActions.Add(action);
    internal void AddShutdownAction(Action<ShutdownContext> action) => _shutdownActions.Add(action);

    internal void AddConfigureAsyncAction(Func<ConfigureContext, Task> action) => _asyncConfigureActions.Add(action);
    internal void AddInitAsyncAction(Func<InitContext, Task> action) => _asyncInitActions.Add(action);
    internal void AddShutdownAsyncAction(Func<ShutdownContext, Task> action) => _asyncShutdownActions.Add(action);

    public override async Task OnConfiguringAsync(ConfigureContext context)
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





    public override void OnCreating(ModuleBuilder moduleBuilder)
    {
        foreach (var action in _moduleBuilderActions)
        {
            action(moduleBuilder);
        }
    }

    public override void OnConfiguring(ConfigureContext context)
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
