using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha;


/// <summary>
/// Represents a successful result that carries a value (non-generic access).
/// </summary>
public interface IRamshaValueSuccessResult : IRamshaSuccessResult
{
    /// <summary>The returned value.</summary>
    object Value { get; }
}

/// <summary>
/// Represents a successful result that carries a value of type <typeparamref name="TValue"/>.
/// </summary>
public interface IRamshaValueSuccessResult<TValue> : IRamshaValueSuccessResult
{
    /// <summary>The returned value of type <typeparamref name="TValue"/>.</summary>
    new TValue Value { get; }
    object IRamshaValueSuccessResult.Value => Value!;
}

/// <summary>
/// Combines typed value success with a strongly-typed result interface for <typeparamref name="T"/>.
/// </summary>
public interface IRamshaValueSuccessResult<T, TValue> : IRamshaValueSuccessResult<TValue>, IRamshaSuccessResult<T>
    where T : IRamshaValueSuccessResult<T, TValue>;

