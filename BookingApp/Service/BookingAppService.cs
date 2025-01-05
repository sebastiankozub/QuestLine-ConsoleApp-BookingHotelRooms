using BookingData;

namespace BookingApp.Service
{
    public abstract class BookingAppService(DataContext dataContext) : IBookingAppService
    {
        protected readonly DataContext _dataContext = dataContext;
    }

    interface IBookingAppService
    {
    }
}
