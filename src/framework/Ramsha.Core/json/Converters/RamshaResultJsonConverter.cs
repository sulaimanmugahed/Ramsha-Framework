
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ramsha;

[AttributeUsage(AttributeTargets.Interface, Inherited = false, AllowMultiple = false)]
internal sealed class IRamshaResultJsonConverterAttribute()
    : JsonConverterAttribute(typeof(RamshaResultJsonConverter))
    ;

internal class RamshaResultJsonConverter : JsonConverter<IRamshaResult>
{
    public override IRamshaResult Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(
        Utf8JsonWriter writer,
        IRamshaResult value,
        JsonSerializerOptions options)
    {
        switch (value)
        {
            case null:
                JsonSerializer.Serialize(writer, (IRamshaResult?)null, options);
                return;
            default:
                var type = value.GetType();
                JsonSerializer.Serialize(writer, value, type, options);
                return;
        }
    }
}