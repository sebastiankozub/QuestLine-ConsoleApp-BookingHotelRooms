using System.Text.Json.Serialization;
using System.Text.Json;

namespace BookingUtils.FrameworkExtensions;

public class ConstrainedLenghtStringConverter(uint min, uint max) : JsonConverter<string>
{
    private readonly uint _minimumLenght = min;
    private readonly uint _maximumLenght = max;

    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
            throw new JsonException("Expected string value.");

        string value = reader.GetString() ?? throw new JsonException("String cannot be empty.");  // null strictly not accepted atm

        if (_minimumLenght != 0)
        {
            if (string.IsNullOrEmpty(value))
                throw new JsonException($"String has to have minimum lenght {_minimumLenght}.");
            if (value.Length < _minimumLenght)
                throw new JsonException($"String has to have minimum lenght {_minimumLenght}.");
        }

        if (_maximumLenght != 0)
        {
            if (value.Length > _maximumLenght)
                throw new JsonException($"String has to have maximum lenght {_maximumLenght}.");
        }

        return value;
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        // TODO validate save also when start using to not to save data that will excpetion app loading then
        writer.WriteStringValue(value);
    }
}






    //public class ConstrainedStringConverterFactory : JsonConverterFactory
    //{
    //    private readonly uint _maximumLength = 0;
    //    private readonly uint _minimumLenght = 0;

    //    public ConstrainedStringConverterFactory(uint minLenght, uint maxLength)
    //    {
    //        _maximumLength = maxLength;
    //        _minimumLenght = minLenght;
    //    }

    //    public override bool CanConvert(Type typeToConvert)
    //    {
    //        return typeToConvert == typeof(string);
    //    }

    //    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    //    {
    //        return new ConstrainedLenghtStringConverter(_minimumLenght, _maximumLength);
    //    }
    //}


