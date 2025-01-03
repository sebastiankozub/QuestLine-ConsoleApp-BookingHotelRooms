using System.Text.Json;
using ConsoleBookingApp.Data.Model;

namespace ConsoleBookingApp.Data;

public class DataContext
{
    public List<Hotel> Hotels { get; private set; } = new List<Hotel>();
    public List<Booking> Bookings { get; private set; } = [];

    public DataContext(string hotelRepositoryFilename, string bookingRepositoryFilename)
    {
        _hotelRepositoryFilename = hotelRepositoryFilename;
        _bookingRepositoryFilename = bookingRepositoryFilename;

        Initialization = InitializeAsync();
    }

    public Task Initialization { get; private set; }

    public async Task SaveAsync() // as we have kind of unit of work pattern then common save method for consistency
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
        var deserializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        using var hotelsReaderr = new StreamReader(_hotelRepositoryFilename);    
        var hotels = await JsonSerializer.DeserializeAsync<List<Hotel>>(hotelsReaderr.BaseStream, deserializeOptions);

        using var bookingsReader = new StreamReader(_bookingRepositoryFilename);
        var bookings = await JsonSerializer.DeserializeAsync<List<Booking>>(bookingsReader.BaseStream, deserializeOptions);

        if (hotels is not null && bookings is not null)
        {
            Hotels = hotels;
            Bookings = bookings;
        }

        //throw new ArgumentException("One or more underlying datafiles are empty or corrupted.");
        //var fs = File.OpenRead(_bookingRepositoryFilename);
    }

    private JsonSerializerOptions GetDeserializerOptions()
    {
        return GetJsonSerializerOptions();
    }

    private JsonSerializerOptions GetJsonSerializerOptions()
    {
        return new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }

    private async Task SaveHotelsToJsonAsync()
    {
        using var hotelsWriter = new StreamWriter(_hotelRepositoryFilename);
        await hotelsWriter.WriteAsync(JsonSerializer.Serialize(Hotels, GetJsonSerializerOptions()));
    }

    private async Task SaveBookingsToJsonAsync()
    {
        using var bookingsWriter = new StreamWriter(_bookingRepositoryFilename);
        await bookingsWriter.WriteAsync(JsonSerializer.Serialize(Bookings, GetJsonSerializerOptions()));
    }
}


