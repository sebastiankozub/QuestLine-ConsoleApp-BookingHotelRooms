using BookingData;
using System.Linq;

namespace BookingApp.Service;

public class RoomAvailabilityService : BookingAppService, IRoomAvailabilityService
{
    public RoomAvailabilityService(DataContext dataContext) : base(dataContext) { }

    public async Task<IEnumerable<RoomAvaialabilityResult>> GetRoomAvailabilityByRoomType(string hotelId, (DateOnly from, DateOnly to) availabilityPerdiod, string roomType)
    {
        var bookings = _dataContext.Bookings;
        var hotels = _dataContext.Hotels;
        
        var dates = ListOfDates(availabilityPerdiod.from, availabilityPerdiod.to);

        var bookingsByDay = dates.Select(date => new
        {
            Date = date,
            BookingCount = bookings
                .Where(b => b.RoomType == roomType && b.HotelId == hotelId && b.Arrival <= date && b.Departure >= date)
                .Count()
        });

        var hotel = hotels.SingleOrDefault(hotel => hotel.Id == hotelId) ?? 
            throw new RoomAvailabilityServiceException($"Hotel with given id {hotelId} not found");

        var numberOfRommsOfType = hotel.Rooms.Count(room => room.RoomType == roomType);

        var hotelAvailability = numberOfRommsOfType == 0 ? 
            throw new RoomAvailabilityServiceException($"Hotel with given id {hotelId} does not offer any room type {roomType}") : numberOfRommsOfType;

        var hotelAvailabilityPerDay = Enumerable.Repeat(hotelAvailability, dates.Count);

        return hotelAvailabilityPerDay
            .Zip(bookingsByDay, (a, b) =>  new RoomAvaialabilityResult { Day = b.Date, AvailabilityCount = a - b.BookingCount });
    }

    public static List<DateOnly> ListOfDates(DateOnly from, DateOnly to)
    {
        var days = new List<DateOnly>();
        for (var day = from; day <= to; day = day.AddDays(1))
        {
            days.Add(day);
        }
        return days;
    }

    public static List<DateOnly> ListOfDates(DateOnly from, int daysCount)
    {
        return  Enumerable.Range(0, daysCount)
            .Select(offset => from.AddDays(offset))
            .ToList();
    }
}

public interface IRoomAvailabilityService
{
    Task<IEnumerable<RoomAvaialabilityResult>> GetRoomAvailabilityByRoomType(string hotelId, (DateOnly from, DateOnly to) availabilityPerdiod, string roomType);
}

public class RoomAvaialabilityResult
{
    public DateOnly Day { get; set; }
    public int AvailabilityCount { get; set; }
}

public class RoomAvailabilityServiceException : Exception
{
    public RoomAvailabilityServiceException()
            : base(nameof(RoomAvailabilityServiceException))
    { }

    public RoomAvailabilityServiceException(string? message)
      : base(message ?? nameof(RoomAvailabilityServiceException))
    { } 
}
