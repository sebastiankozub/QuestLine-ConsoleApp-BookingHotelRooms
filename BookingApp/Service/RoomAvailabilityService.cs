using BookingData;
using System.Linq;

namespace BookingApp.Service;

public class RoomAvailabilityService(DataContext dataContext) : BookingAppService(dataContext), IRoomAvailabilityService
{
    public async Task<IEnumerable<RoomAvaialabilityResult>> GetRoomAvailabilityByRoomType(string hotelId, (DateOnly from, DateOnly to) availabilityPerdiod, string roomType, bool aggregated = false)
    {
        var bookings = _dataContext.Bookings;
        var hotels = _dataContext.Hotels;
        
        return await Task<IEnumerable<RoomAvaialabilityResult>>.Run(() =>
        {
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

            var roomsOfGivenType = hotel.Rooms.Count(room => room.RoomType == roomType);

            var hotelAvailability = roomsOfGivenType == 0 ?
                throw new RoomAvailabilityServiceException($"Hotel with given id {hotelId} does not offer any room type {roomType}") : roomsOfGivenType;

            var hotelAvailabilityPerDay = Enumerable.Repeat(hotelAvailability, dates.Count);

            var roomAvailability = hotelAvailabilityPerDay
                .Zip(bookingsByDay, (a, b) => new RoomAvaialabilityResult { Day = b.Date, SameCountPeriod = 1, RoomAvailabilityCount = a - b.BookingCount });

            if (aggregated)
            {
                return roomAvailability.Aggregate(new List<RoomAvaialabilityResult>(), (acc, current) =>
                {
                    if (acc.Count == 0 || acc.Last().RoomAvailabilityCount != current.RoomAvailabilityCount)
                    {
                        acc.Add(current);
                    }
                    else
                    {
                        var lastAdded = acc.Last();
                        var sameCountPeriod = lastAdded.SameCountPeriod;
                        sameCountPeriod++;

                        for (int i = (int)(sameCountPeriod - 1); i > 0; i--)
                            acc[acc.Count - i].SameCountPeriod = sameCountPeriod;

                        current.SameCountPeriod = sameCountPeriod;

                        acc.Add(current);
                    }
                    return acc;
                });
            }
            else
                return roomAvailability;
        });
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
    Task<IEnumerable<RoomAvaialabilityResult>> GetRoomAvailabilityByRoomType(
        string hotelId, (DateOnly from, DateOnly to) availabilityPerdiod, string roomType, bool aggregated = false);
}

public class RoomAvaialabilityResult
{
    public DateOnly Day { get; set; }
    public uint SameCountPeriod { get; set; }
    public int RoomAvailabilityCount { get; set; }
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
