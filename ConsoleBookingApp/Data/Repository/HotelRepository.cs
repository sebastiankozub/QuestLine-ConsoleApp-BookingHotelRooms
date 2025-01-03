using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using ConsoleBookingApp.Data;

namespace ConsoleBookingApp.Data.Repository;

public class HotelRepository : IRepository<Hotel>
{
    private readonly string _jsonFilePath;
    private readonly List<Hotel> _hotels;

    public HotelRepository(string jsonFilePath)
    {
        _jsonFilePath = jsonFilePath;
        _hotels = new List<Hotel>();
    }

    public HotelRepository()
    {
        _hotels = new List<Hotel>();
    }

    public HotelRepository(List<Hotel> hotels)
    {
        _hotels = hotels;
    }

    public async Task<IEnumerable<Hotel>> GetAllAsync()
    {
        return _hotels;
    }

    public async Task<Hotel> GetByIdAsync(int id)
    {
        await LoadHotelsFromJsonAsync();
        return _hotels.FirstOrDefault(h => h.Id == id);
    }

    public async Task AddAsync(Hotel entity)
    {
        await LoadHotelsFromJsonAsync();
        entity.Id = _hotels.Any() ? _hotels.Max(h => h.Id) + 1 : 1; // Auto-increment ID
        _hotels.Add(entity);
        await SaveHotelsToJsonAsync();
    }

    public async Task UpdateAsync(Hotel entity)
    {
        await LoadHotelsFromJsonAsync();
        var existingHotel = _hotels.FirstOrDefault(h => h.Id == entity.Id);
        if (existingHotel != null)
        {
            int index = _hotels.IndexOf(existingHotel);
            _hotels[index] = entity; // Replace the existing hotel
            await SaveHotelsToJsonAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        await LoadHotelsFromJsonAsync();
        var hotelToRemove = _hotels.FirstOrDefault(h => h.Id == id);
        if (hotelToRemove != null)
        {
            _hotels.Remove(hotelToRemove);
            await SaveHotelsToJsonAsync();
        }
    }
}