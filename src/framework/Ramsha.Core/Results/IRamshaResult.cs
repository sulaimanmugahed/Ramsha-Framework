using System.Text.Json.Serialization;

namespace Ramsha;

[IRamshaResultJsonConverter]
public interface IRamshaResult
{
    public RamshaResultStatus Status { get; }
    RamshaErrorResult? Error { get; }
    bool Succeeded { get; }
}




