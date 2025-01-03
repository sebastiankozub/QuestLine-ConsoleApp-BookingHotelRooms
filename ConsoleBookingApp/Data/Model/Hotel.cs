using System.Text.Json.Serialization;

namespace ConsoleBookingApp.Data.Model;

public class Room
{
    [JsonPropertyName("roomId")]
    public required string RoomId { get; set; }
    [JsonPropertyName("roomType")]
    public required string RoomType { get; set; }
}
//        "roomType": "SGL",
//        "roomId": "101"

public class RoomType
{
    public required string Code { get; set; }
    public required string Description { get; set; }
    public required List<string> Amenities { get; set; }
    public required List<string> Features { get; set; }
}
//        "code": "SGL",
//        "description": "Single Room",
//        "amenities": [
//          "WiFi",
//          "TV"
//        ],
//        "features": ["Non-smoking"]

public class Hotel
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required List<Room> Rooms { get; set; }
    public required List<RoomType> RoomTypes { get; set; }
}
//    "id": "H1",
//    "name": "Hotel California",
//    "roomTypes": [
//      {
//        "code": "SGL",
//        "description": "Single Room",
//        "amenities": [
//          "WiFi",
//          "TV"
//        ],
//        "features": ["Non-smoking"]
//      },
//      {
//    "code": "DBL",
//        "description": "Double Room",
//        "amenities": [
//          "WiFi",
//          "TV",
//          "Minibar"
//        ],
//        "features": ["Non-smoking", "Sea View"]
//      }
//    ],
//    "rooms": [
//      {
//        "roomType": "SGL",
//        "roomId": "101"
//      },
//      {
//    "roomType": "SGL",
//        "roomId": "102"
//      },
//      {
//    "roomType": "DBL",
//        "roomId": "201"
//      },
//      {
//    "roomType": "DBL",
//        "roomId": "202"
//      }
//    ]
