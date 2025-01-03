using ConsoleBookingApp.FrameworkExtensions;
using System.Text.Json.Serialization;

namespace ConsoleBookingApp.Data.Model;

public class Booking
{
    public required string HotelId { get; set; }
    public required string RoomRate { get; set; }
    public required string RoomType { get; set; }

    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateOnly Arrival { get; set; }
    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateOnly Departure { get; set; }
}




