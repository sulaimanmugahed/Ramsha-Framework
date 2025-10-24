using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Ramsha;

public class ModuleLifecycleOptions
{
    public ITypeList<IModuleLifecycleContributor> Contributors { get; }

    public ModuleLifecycleOptions()
    {
        Contributors = new TypeList<IModuleLifecycleContributor>();
    }
}

public interface IOnAppShutdown
{
    Task OnShutdownAsync(ShutdownContext context);

    void OnShutdown(ShutdownContext context);
}

public interface IOnAppInit
{
    Task OnInitAsync(InitContext context);

    void OnInit(InitContext context);
}

public class OnAppInitModuleLifecycleContributor : ModuleLifecycleContributorBase
{
    public async override Task InitAsync(InitContext context, IRamshaModule module)
    {
        if (module is IOnAppInit onApplicationInitialization)
        {
            await onApplicationInitialization.OnInitAsync(context);
        }
    }

    public override void Init(InitContext context, IRamshaModule module)
    {
        (module as IOnAppInit)?.OnInit(context);
    }
}

public class OnAppShutdownModuleLifecycleContributor : ModuleLifecycleContributorBase
{
    public async override Task ShutdownAsync(ShutdownContext context, IRamshaModule module)
    {
        if (module is IOnAppShutdown onApplicationShutdown)
        {
            await onApplicationShutdown.OnShutdownAsync(context);
        }
    }

    public override void Shutdown(ShutdownContext context, IRamshaModule module)
    {
        (module as IOnAppShutdown)?.OnShutdown(context);
    }
}



public abstract class ModuleLifecycleContributorBase : IModuleLifecycleContributor
{
    public virtual Task InitAsync(InitContext context, IRamshaModule module)
    {
        return Task.CompletedTask;
    }

    public virtual void Init(InitContext context, IRamshaModule module)
    {
    }

    public virtual Task ShutdownAsync(ShutdownContext context, IRamshaModule module)
    {
        return Task.CompletedTask;
    }

    public virtual void Shutdown(ShutdownContext context, IRamshaModule module)
    {
    }
}
public interface IModuleLifecycleContributor
{
    Task InitAsync(InitContext context, IRamshaModule module);

    void Init(InitContext context, IRamshaModule module);

    Task ShutdownAsync(ShutdownContext context, IRamshaModule module);

    void Shutdown(ShutdownContext context, IRamshaModule module);
}

public interface ITypeList : ITypeList<object>
{

}


public interface ITypeList<in TBaseType> : IList<Type>
{
    /// <summary>
    /// Adds a type to list.
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    void Add<T>() where T : TBaseType;

    /// <summary>
    /// Adds a type to list if it's not already in the list.
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    bool TryAdd<T>() where T : TBaseType;

    /// <summary>
    /// Checks if a type exists in the list.
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <returns></returns>
    bool Contains<T>() where T : TBaseType;

    /// <summary>
    /// Removes a type from list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    void Remove<T>() where T : TBaseType;
}

public class TypeList : TypeList<object>, ITypeList
{
}

/// <summary>
/// Extends <see cref="List{Type}"/> to add restriction a specific base type.
/// </summary>
/// <typeparam name="TBaseType">Base Type of <see cref="Type"/>s in this list</typeparam>
public class TypeList<TBaseType> : ITypeList<TBaseType>
{
    /// <summary>
    /// Gets the count.
    /// </summary>
    /// <value>The count.</value>
    public int Count => _typeList.Count;

    /// <summary>
    /// Gets a value indicating whether this instance is read only.
    /// </summary>
    /// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
    public bool IsReadOnly => false;

    /// <summary>
    /// Gets or sets the <see cref="Type"/> at the specified index.
    /// </summary>
    /// <param name="index">Index.</param>
    public Type this[int index]
    {
        get { return _typeList[index]; }
        set
        {
            CheckType(value);
            _typeList[index] = value;
        }
    }

    private readonly List<Type> _typeList;

    /// <summary>
    /// Creates a new <see cref="TypeList{T}"/> object.
    /// </summary>
    public TypeList()
    {
        _typeList = new List<Type>();
    }

    /// <inheritdoc/>
    public void Add<T>() where T : TBaseType
    {
        _typeList.Add(typeof(T));
    }

    public bool TryAdd<T>() where T : TBaseType
    {
        if (Contains<T>())
        {
            return false;
        }

        Add<T>();
        return true;
    }

    /// <inheritdoc/>
    public void Add(Type item)
    {
        CheckType(item);
        _typeList.Add(item);
    }

    /// <inheritdoc/>
    public void Insert(int index, Type item)
    {
        CheckType(item);
        _typeList.Insert(index, item);
    }

    /// <inheritdoc/>
    public int IndexOf(Type item)
    {
        return _typeList.IndexOf(item);
    }

    /// <inheritdoc/>
    public bool Contains<T>() where T : TBaseType
    {
        return Contains(typeof(T));
    }

    /// <inheritdoc/>
    public bool Contains(Type item)
    {
        return _typeList.Contains(item);
    }

    /// <inheritdoc/>
    public void Remove<T>() where T : TBaseType
    {
        _typeList.Remove(typeof(T));
    }

    /// <inheritdoc/>
    public bool Remove(Type item)
    {
        return _typeList.Remove(item);
    }

    /// <inheritdoc/>
    public void RemoveAt(int index)
    {
        _typeList.RemoveAt(index);
    }

    /// <inheritdoc/>
    public void Clear()
    {
        _typeList.Clear();
    }

    /// <inheritdoc/>
    public void CopyTo(Type[] array, int arrayIndex)
    {
        _typeList.CopyTo(array, arrayIndex);
    }

    /// <inheritdoc/>
    public IEnumerator<Type> GetEnumerator()
    {
        return _typeList.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _typeList.GetEnumerator();
    }

    private static void CheckType(Type item)
    {
        if (!typeof(TBaseType).GetTypeInfo().IsAssignableFrom(item))
        {
            throw new ArgumentException($"Given type ({item.AssemblyQualifiedName}) should be instance of {typeof(TBaseType).AssemblyQualifiedName} ", nameof(item));
        }
    }
}