using BookingData;

namespace BookingApp.Service;

public class RoomAvailabilityService : BookingAppService, IRoomAvailabilityService
{
    public RoomAvailabilityService(DataContext dataContext) : base(dataContext) { }

    public IEnumerable<RoomAvaialabilityResult> GetRoomAvailabilityByType(string hotelId, (DateOnly from, DateOnly to) availabilityPerdiod, string roomType)
    {
        var bookings = _dataContext.Bookings;
        var hotels = _dataContext.Hotels;

        var dates = ListOfDates(availabilityPerdiod.from, availabilityPerdiod.to);

        var bookingsPerDay = dates.Select(dayDate => new
        {
            Date = dayDate,
            BookingCount = bookings
                .Where(b => b.RoomType == roomType && b.HotelId == hotelId && availabilityPerdiod.from <= dayDate && availabilityPerdiod.to >= dayDate)
                .Count()
        });

        var availabilityPerDay = hotels.SingleOrDefault(hotel => hotel.Id == hotelId)?
            .Rooms.Count(room => room.RoomType == roomType);

        return new List<RoomAvaialabilityResult>();
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
    // Availability(H1,         20240901, SGL)  - one day
    // Availability(H1, 20240901-20240903, DBL)   - period

    IEnumerable<RoomAvaialabilityResult> GetRoomAvailabilityByType(string hotelId, (DateOnly from, DateOnly to) availabilityPerdiod, string roomType);

    // the program should give the availability count for the specified room type and        date range.
    // Note:
    // hotels sometimes accept overbookings so the value can be negative to indicate
    // that the hotel is over capacity for that room type.    
}

public class RoomAvaialabilityResult
{
    public DateOnly Day { get; set; }
    public int AvailabilityCount { get; set; }
}
