using BookingData;

namespace BookingApp.Service
{
    public abstract class BookingAppService(IDataContext dataContext) : IBookingAppService
    {
        protected readonly IDataContext _dataContext = dataContext;
    }

    interface IBookingAppService
    {
    }
}
