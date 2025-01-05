using System.Text.Json;
using BookingData.Model;

namespace BookingData;

public class DataContext
{
    // when JsonFileDataStorage will be added List<> will be changed into generic repository
    // to handle save to underlying storage or save on the fly when entity added
    public List<Hotel> Hotels { get; private set; } = [];  
    public List<Booking> Bookings { get; private set; } = [];

    public DataContext(string hotelRepositoryFilename, string bookingRepositoryFilename)
    {
        _hotelRepositoryFilename = hotelRepositoryFilename;
        _bookingRepositoryFilename = bookingRepositoryFilename;

        Initialization = InitializeAsync();
    }

    public Task Initialization { get; private set; }

    public async Task SaveAsync() // unit of work pattern so common Save method for consistency and future functionalities
    {
        await Task.WhenAll(
            SaveHotelsToJsonAsync(),
            SaveBookingsToJsonAsync()
        );
    }

    private readonly string _hotelRepositoryFilename;
    private readonly string _bookingRepositoryFilename;

    private async Task InitializeAsync()
    {
        using var hotelsReaderr = new StreamReader(_hotelRepositoryFilename);    
        var hotels = await JsonSerializer.DeserializeAsync<List<Hotel>>(hotelsReaderr.BaseStream, JsonDataFileSerializerOptions());

        using var bookingsReader = new StreamReader(_bookingRepositoryFilename);
        var bookings = await JsonSerializer.DeserializeAsync<List<Booking>>(bookingsReader.BaseStream, JsonDataFileSerializerOptions());

        if (hotels is not null && bookings is not null)
        {
            Hotels = hotels;
            Bookings = bookings;
        }
    }

    private static JsonSerializerOptions JsonDataFileSerializerOptions()
    {
        return new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }

    private async Task SaveHotelsToJsonAsync()  // looks like Save() chunks can be done as a generic methods
    {
        using var hotelsWriter = new StreamWriter(_hotelRepositoryFilename);
        await hotelsWriter.WriteAsync(JsonSerializer.Serialize(Hotels, JsonDataFileSerializerOptions()));
    }

    private async Task SaveBookingsToJsonAsync()
    {
        using var bookingsWriter = new StreamWriter(_bookingRepositoryFilename);
        await bookingsWriter.WriteAsync(JsonSerializer.Serialize(Bookings, JsonDataFileSerializerOptions()));
    }
}


