using BookingData;

namespace BookingApp.Service;

// NOT used due to using same service on both handlers - same data query just different presentation
public class SearchService : BookingAppService, ISearchService
{
    public SearchService(DataContext dataContext) : base(dataContext) { }

    public async Task<IEnumerable<SearchResult>> GetRoomSearchAsync(string hotelId, (DateOnly from, DateOnly to) searchPerdiod, string roomType)
    {
        throw new NotImplementedException("SearchService not used and implemented");
        //return await Task.FromResult(new List<SearchResult>());
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