using ConsoleBookingApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;

namespace ConsoleBookingApp.Data.Repository;

public class BookingRepository : IRepository<Booking>
{
    private readonly string _jsonFilePath;
    private List<Booking> _bookings;

    public BookingRepository(string jsonFilePath)
    {
        _jsonFilePath = jsonFilePath;
        _bookings = new List<Booking>();
    }

    public BookingRepository()
    {
        _bookings = new List<Booking>();
    }

    public BookingRepository(List<Booking> bookings)
    {
        _bookings = bookings;
    }

    private async Task LoadHotelsFromJsonAsync()
    {
        if (_bookings.Any()) return;

        try
        {
            if (File.Exists(_jsonFilePath))
            {
                using (FileStream openStream = File.OpenRead(_jsonFilePath))
                {
                    _bookings = await JsonSerializer.DeserializeAsync<List<Booking>>(openStream) ?? new List<Booking>();
                }
            }
            else
            {
                Console.WriteLine($"Warning: JSON file not found at {_jsonFilePath}. Creating an empty list.");
                await File.WriteAllTextAsync(_jsonFilePath, "[]");
            }
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Error reading JSON file: {ex.Message}");
            _bookings = new List<Booking>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred during JSON loading: {ex.Message}");
            _bookings = new List<Booking>();
        }
    }

    private async Task SaveHotelsToJsonAsync()
    {
        using (FileStream createStream = File.Create(_jsonFilePath))
        {
            await JsonSerializer.SerializeAsync(createStream, _bookings, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    public async Task<IEnumerable<Booking>> GetAllAsync()
    {
        await LoadHotelsFromJsonAsync();
        return _bookings;
    }

    public async Task<Booking> GetByIdAsync(int id)
    {
        await LoadHotelsFromJsonAsync();
        return _bookings.FirstOrDefault(h => h.HotelId == id);
    }

    public async Task AddAsync(Booking entity)
    {
        await LoadHotelsFromJsonAsync();
        entity.HotelId = _bookings.Any() ? _bookings.Max(h => h.HotelId) + 1 : 1; // Auto-increment ID
        _bookings.Add(entity);
        await SaveHotelsToJsonAsync();
    }

    public async Task UpdateAsync(Booking entity)
    {
        await LoadHotelsFromJsonAsync();
        var existingHotel = _bookings.FirstOrDefault(h => h.HotelId == entity.HotelId);
        if (existingHotel != null)
        {
            int index = _bookings.IndexOf(existingHotel);
            _bookings[index] = entity; // Replace the existing hotel
            await SaveHotelsToJsonAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        await LoadHotelsFromJsonAsync();
        var hotelToRemove = _bookings.FirstOrDefault(h => h.HotelId == id);
        if (hotelToRemove != null)
        {
            _bookings.Remove(hotelToRemove);
            await SaveHotelsToJsonAsync();
        }
    }

}