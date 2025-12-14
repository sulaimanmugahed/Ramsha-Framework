using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha.Core.Modularity.Contexts;

namespace Ramsha;

public class DefaultStartupModuleBuilder
{
    private readonly DefaultStartupModule _module = new();

    public DefaultStartupModuleBuilder Register(Action<RegisterContext> action)
    {
        _module.AddRegisterAction(action);
        return this;
    }

    public DefaultStartupModuleBuilder Prepare(Action<PrepareContext> action)
        => Prepare(ctx =>
        {
            action(ctx);
            return Task.CompletedTask;
        });

    public DefaultStartupModuleBuilder Prepare(Func<PrepareContext, Task> action)
    {
        _module.AddPrepareAction(action);
        return this;
    }

    public DefaultStartupModuleBuilder BuildServices(Action<BuildServicesContext> action)
        => BuildServices(ctx =>
        {
            action(ctx);
            return Task.CompletedTask;
        });

    public DefaultStartupModuleBuilder BuildServices(Func<BuildServicesContext, Task> action)
    {
        _module.AddBuildServicesAction(action);
        return this;
    }

    public DefaultStartupModuleBuilder OnInit(Action<InitContext> action)
        => OnInit(ctx =>
        {
            action(ctx);
            return Task.CompletedTask;
        });

    public DefaultStartupModuleBuilder OnInit(Func<InitContext, Task> action)
    {
        _module.AddInitAction(action);
        return this;
    }

    public DefaultStartupModuleBuilder OnShutdown(Action<ShutdownContext> action)
        => OnShutdown(ctx =>
        {
            action(ctx);
            return Task.CompletedTask;
        });

    public DefaultStartupModuleBuilder OnShutdown(Func<ShutdownContext, Task> action)
    {
        _module.AddShutdownAction(action);
        return this;
    }

    internal DefaultStartupModule Build() => _module;
}


public sealed class DefaultStartupModule : RamshaModule
{
    private readonly List<Action<RegisterContext>> _register = new();
    private readonly List<Func<PrepareContext, Task>> _prepare = new();
    private readonly List<Func<BuildServicesContext, Task>> _buildServices = new();
    private readonly List<Func<InitContext, Task>> _init = new();
    private readonly List<Func<ShutdownContext, Task>> _shutdown = new();

    internal void AddRegisterAction(Action<RegisterContext> action) => _register.Add(action);
    internal void AddPrepareAction(Func<PrepareContext, Task> action) => _prepare.Add(action);
    internal void AddBuildServicesAction(Func<BuildServicesContext, Task> action) => _buildServices.Add(action);
    internal void AddInitAction(Func<InitContext, Task> action) => _init.Add(action);
    internal void AddShutdownAction(Func<ShutdownContext, Task> action) => _shutdown.Add(action);

    public override void Register(RegisterContext context)
    {
        foreach (var action in _register)
            action(context);
    }

    public override async Task PrepareAsync(PrepareContext context)
    {
        foreach (var action in _prepare)
            await action(context);
    }

    public override async Task BuildServicesAsync(BuildServicesContext context)
    {
        foreach (var action in _buildServices)
            await action(context);
    }

    public override async Task OnInitAsync(InitContext context)
    {
        foreach (var action in _init)
            await action(context);
    }

    public override async Task OnShutdownAsync(ShutdownContext context)
    {
        foreach (var action in _shutdown)
            await action(context);
    }
}