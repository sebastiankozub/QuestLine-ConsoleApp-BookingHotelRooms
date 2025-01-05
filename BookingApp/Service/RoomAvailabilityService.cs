using BookingData;
using System.Xml.Linq;

namespace BookingApp.Service;

public class RoomAvailabilityService(IDataContext dataContext) : BookingAppService(dataContext), IRoomAvailabilityService
{
    public async Task<IEnumerable<RoomAvaialabilityServiceResult>> GetRoomAvailabilityByRoomType(string hotelId, (DateOnly from, DateOnly to) availabilityPerdiod, string roomType, bool aggregated = false)
    {
        var bookings = _dataContext.Bookings;
        var hotels = _dataContext.Hotels;
        
        return await Task<IEnumerable<RoomAvaialabilityServiceResult>>.Run(() =>
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
                throw new RoomAvailabilityServiceException($"Hotel with given id {hotelId} does not offer any room of type {roomType}") : roomsOfGivenType;

            var hotelAvailabilityPerDay = Enumerable.Repeat(hotelAvailability, dates.Count);

            var roomAvailability = hotelAvailabilityPerDay
                .Zip(bookingsByDay, (a, b) => new RoomAvaialabilityServiceResult(b.Date, 1, a - b.BookingCount));

            if (aggregated)            
                return AggregateAvailabilityByDate(roomAvailability);            
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

    private static List<RoomAvaialabilityServiceResult> AggregateAvailabilityByDate(IEnumerable<RoomAvaialabilityServiceResult> roomAvailability)
    {
        return roomAvailability.Aggregate(new List<RoomAvaialabilityServiceResult>(), (acc, current) =>
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

                RoomAvaialabilityServiceResult roomAvaialabilityServiceResult = new RoomAvaialabilityServiceResult();

                for (int i = (int)(sameCountPeriod - 1); i > 0; i--)
                {
                    var sameCountGroupItem = acc[acc.Count - i];
                    roomAvaialabilityServiceResult = new RoomAvaialabilityServiceResult(sameCountGroupItem.Day, sameCountPeriod, sameCountGroupItem.RoomAvailabilityCount);
                    acc[acc.Count - i] = roomAvaialabilityServiceResult;
                }

                acc.Add(new RoomAvaialabilityServiceResult(
                    current.Day,
                    roomAvaialabilityServiceResult.SameCountPeriod, 
                    current.RoomAvailabilityCount ));
            }
            return acc;
        });
    }
}

public interface IRoomAvailabilityService
{
    Task<IEnumerable<RoomAvaialabilityServiceResult>> GetRoomAvailabilityByRoomType(
        string hotelId, (DateOnly from, DateOnly to) availabilityPerdiod, string roomType, bool aggregated = false);
}

public struct RoomAvaialabilityServiceResult(DateOnly day, uint sameCountPeriod, int roomAvailabilityCount)
{
    public DateOnly Day = day;
    public uint SameCountPeriod = sameCountPeriod;
    public int RoomAvailabilityCount = roomAvailabilityCount;
    //public long RoomAvaialabilityUniqueCode = (new Random()).NextInt64();
}


//public class RoomAvaialabilityServiceResult : IEquatable<RoomAvaialabilityServiceResult>
//{
//    public DateOnly Day { get; set; }
//    public uint SameCountPeriod { get; set; }
//    public int RoomAvailabilityCount { get; set; }
//    public long RoomUniqueCode = (new Random()).NextInt64();

//    public bool Equals(RoomAvaialabilityServiceResult? other)
//    {
//        if (other == null) 
//            return false;

//        return Day == other.Day 
//            && SameCountPeriod == other.SameCountPeriod 
//            && RoomAvailabilityCount == other.RoomAvailabilityCount;
//    }

//    public override bool Equals(object? obj)
//    {
//        return Equals(obj as RoomAvaialabilityServiceResult);
//    }

//    public override int GetHashCode()
//    {
//        return HashCode.Combine<DateOnly, uint, int, long>(Day, SameCountPeriod, RoomAvailabilityCount, RoomUniqueCode);
//    }
//}

public class RoomAvailabilityServiceException : Exception
{
    public RoomAvailabilityServiceException()
            : base(nameof(RoomAvailabilityServiceException)) { }

    public RoomAvailabilityServiceException(string? message)
      : base(message ?? nameof(RoomAvailabilityServiceException)) { } 
}