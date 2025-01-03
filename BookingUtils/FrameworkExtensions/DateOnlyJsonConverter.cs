using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace BookingUtils.FrameworkExtensions;

public class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    private const string Format = "yyyyMMdd";

    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var currentToken = reader.GetString();
        return currentToken == null ? default : DateOnly.ParseExact(currentToken, Format, CultureInfo.InvariantCulture);
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(Format, CultureInfo.InvariantCulture));
    }
}
