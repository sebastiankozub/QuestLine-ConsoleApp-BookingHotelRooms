using ConsoleBookingApp.Data;

namespace ConsoleBookingApp.Core.Service;

public class AvailabilityService : IAvailabilityService
{
    private readonly DataContext _dataContext;

    public AvailabilityService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public IEnumerable<AvaialabilityResult> GetAvailability(string hotelId, (DateOnly from, DateOnly to) availabilityPerdiod, string roomType)
    {
        return new List<AvaialabilityResult>();
    }
}

interface IAvailabilityService : IBookingService
{
    // Availability(H1,         20240901, SGL)  - one day
    // Availability(H1, 20240901-20240903, DBL)   - period

    IEnumerable<AvaialabilityResult> GetAvailability(string hotelId, (DateOnly from, DateOnly to) availabilityPerdiod, string roomType);

    // the program should give the availability count for the specified room type and        date range.
    // Note:
    // hotels sometimes accept overbookings so the value can be negative to indicate
    // that the hotel is over capacity for that room type.    
}

public class AvaialabilityResult
{
    public DateOnly Day { get; set; }
    public int AvailabilityCount { get; set; }
}
