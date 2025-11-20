using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha;

public class RamshaResult
{
    public bool Success { get; set; }
    public List<RamshaError>? Errors { get; set; }

    public static RamshaResult Ok()
        => new() { Success = true };

    public static RamshaResult Failure()
        => new() { Success = false };

    public static RamshaResult Failure(RamshaError error)
        => new() { Success = false, Errors = [error] };

    public static RamshaResult Failure(IEnumerable<RamshaError> errors)
        => new() { Success = false, Errors = errors.ToList() };

    public static implicit operator RamshaResult(RamshaError error)
        => new() { Success = false, Errors = [error] };

    public static implicit operator RamshaResult(List<RamshaError> errors)
        => new() { Success = false, Errors = errors };

    public RamshaResult AddError(RamshaError error)
    {
        Errors ??= [];
        Errors.Add(error);
        Success = false;
        return this;
    }
}

public class RamshaResult<TData> : RamshaResult
{
    public TData Data { get; set; }

    public static RamshaResult<TData> Ok(TData data)
        => new() { Success = true, Data = data };
    public new static RamshaResult<TData> Failure()
        => new() { Success = false };

    public new static RamshaResult<TData> Failure(RamshaError error)
        => new() { Success = false, Errors = [error] };

    public new static RamshaResult<TData> Failure(IEnumerable<RamshaError> errors)
        => new() { Success = false, Errors = errors.ToList() };

    public static implicit operator RamshaResult<TData>(TData data)
        => new() { Success = true, Data = data };

    public static implicit operator RamshaResult<TData>(RamshaError error)
        => new() { Success = false, Errors = [error] };

    public static implicit operator RamshaResult<TData>(List<RamshaError> errors)
        => new() { Success = false, Errors = errors };
}

public class RamshaError
{
    public RamshaError(string code, string? description = null, string? source = null)
    {
        Code = code;
        Description = description;
        Source = source;
    }

    public static RamshaError Create(string code, string? description = null, string? source = null)
    {
        return new RamshaError(code, description, source);
    }
    public string Code { get; set; }
    public string? Description { get; set; }
    public string? Source { get; set; }
}