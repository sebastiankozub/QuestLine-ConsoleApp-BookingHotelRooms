using BookingData;

namespace BookingApp.Service;

// NOT used due to using same service on both handlers - same data query just different presentation
public class SearchService(DataContext dataContext) : BookingAppService(dataContext), ISearchService
{
    public async Task<IEnumerable<SearchResult>> GetRoomSearchAsync(string hotelId, (DateOnly from, DateOnly to) searchPerdiod, string roomType)
    {
        await Task.FromResult(1);
        throw new NotImplementedException("SearchService not used and implemented");
    }
}

interface ISearchService
{
    Task<IEnumerable<SearchResult>> GetRoomSearchAsync(string hotelId, (DateOnly from, DateOnly to) searchPerdiod, string roomType);
}

public class SearchResult
{
    public (DateOnly from, DateOnly to) SearchPeriod { get; set; }
    public int SearchCount { get; set; }
}