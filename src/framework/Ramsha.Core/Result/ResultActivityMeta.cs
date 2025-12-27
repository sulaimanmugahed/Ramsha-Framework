using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ramsha;


/// <summary>
/// Use to store <see cref="System.Diagnostics.Activity"/> information where the Result was created.
/// </summary>
/// <param name="Activity"></param>
public readonly partial record struct ResultActivityMeta(
    [property: JsonIgnore] Activity Activity
)
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Id { get; } = Activity.Id;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ActivityTraceId? TraceId { get; } = Activity.TraceId;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ParentId { get; } = Activity.ParentId;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<KeyValuePair<string, string?>>? Tags { get; }
        = Activity.Tags.Any() ? Activity.Tags : null;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<KeyValuePair<string, string?>>? Baggage { get; }
        = Activity.Baggage.Any() ? Activity.Baggage : null;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<ResultActivityMeta>? Parents { get; init; } = null;
}
;

partial record struct ResultActivityMeta
{
    public static IEnumerable<ResultActivityMeta> BuildActivityParents(Activity activity)
    {
        while (activity.Parent is not null)
        {
            activity = activity.Parent;
            yield return new(activity);
        }
    }
}
