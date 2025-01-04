using BookingData;

namespace BookingApp.Service
{
    public abstract class BookingAppService : IBookingAppService
    {
        protected readonly DataContext _dataContext;

        public BookingAppService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
    }

    interface IBookingAppService
    {



    }
}
