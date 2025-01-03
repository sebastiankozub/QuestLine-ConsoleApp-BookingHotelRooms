namespace ConsoleBookingApp.Core.Service;

public class SearchService : ISearchService
{
    public IEnumerable<SearchResult> GetSearch(string hotelId, (DateOnly from, DateOnly to) searchPerdiod, string roomType)
    {
        return new List<SearchResult>();
    }
}

interface ISearchService : IBookingService
{
    IEnumerable<SearchResult> GetSearch(string hotelId, (DateOnly from, DateOnly to) searchPerdiod, string roomType);

// Search(H1,  365, SGL)

//The program should return a comma separated list of date ranges and availability where the room is available.
// In this example, “365” is the number of days ahead to query for availability.
//If there is no availability the program should return an empty line.

//Example
// output:  
// (20241101-20241103,2),
// (20241203-20241210,1) 

}

public class SearchResult
{
    public (DateOnly from, DateOnly to) SearchPeriod { get; set; }
    public int SearchCount { get; set; }
}