using BookingUtils.FrameworkExtensions;
using System.Text.Json.Serialization;

namespace BookingData.Model;

public class Booking
{
    [BookingAppIdentifierLenght(2, 10)]
    public required string HotelId { get; set; }

    public string? RoomRate { get; set; }

    [BookingAppIdentifierLenght(2,10)]
    public required string RoomType { get; set; }

    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateOnly Arrival { get; set; }

    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateOnly Departure { get; set; }
}




