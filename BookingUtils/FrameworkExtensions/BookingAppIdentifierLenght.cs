using System.Text.Json.Serialization;

namespace BookingUtils.FrameworkExtensions;

/// <summary>
/// Given 0 as minimum or maximum will release the constraint
/// </summary>
/// <param name="min"></param>
/// <param name="max"></param>
public class BookingAppIdentifierLenght(uint min, uint max) : JsonConverterAttribute
{
    private readonly uint _minimumLenght = min;
    private readonly uint _maximumLenght = max;

    public override JsonConverter CreateConverter(Type typeToConvert)
    {
        if (typeToConvert != typeof(string))
        {
            throw new ArgumentException(
                $"This converter only works with string type but it was used on {typeToConvert.Name}.");
        }

        return new ConstrainedLenghtStringConverter(_minimumLenght, _maximumLenght);
    }
}
