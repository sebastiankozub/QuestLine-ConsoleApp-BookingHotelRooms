using BookingData;
using BookingApp.Service;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApp.Tests
{
    [TestFixture]
    public class RoomAvailabilityServiceTests
    {
        private Mock<DataContext> _mockDataContext;
        private RoomAvailabilityService _roomAvailabilityService;

        [SetUp]
        public void Setup()
        {
            _mockDataContext = new Mock<DataContext>();
            _roomAvailabilityService = new RoomAvailabilityService(_mockDataContext.Object);
        }

        [Test]
        public async Task GetRoomAvailabilityByRoomType_ShouldReturnRoomAvailability()
        {
            // Arrange
            string hotelId = "123";
            var availabilityPeriod = (from: new DateOnly(2022, 1, 1), to: new DateOnly(2022, 1, 7));
            string roomType = "Standard";
            bool aggregated = false;

            var bookings = new List<Booking>
            {
                new Booking { RoomType = "Standard", HotelId = "123", Arrival = new DateOnly(2022, 1, 2), Departure = new DateOnly(2022, 1, 4) },
                new Booking { RoomType = "Standard", HotelId = "123", Arrival = new DateOnly(2022, 1, 5), Departure = new DateOnly(2022, 1, 6) },
                new Booking { RoomType = "Deluxe", HotelId = "123", Arrival = new DateOnly(2022, 1, 3), Departure = new DateOnly(2022, 1, 5) },
                new Booking { RoomType = "Standard", HotelId = "456", Arrival = new DateOnly(2022, 1, 1), Departure = new DateOnly(2022, 1, 3) }
            };

            var hotels = new List<Hotel>
            {
                new Hotel { Id = "123", Rooms = new List<Room> { new Room { RoomType = "Standard" }, new Room { RoomType = "Deluxe" } } },
                new Hotel { Id = "456", Rooms = new List<Room> { new Room { RoomType = "Standard" } } }
            };

            _mockDataContext.SetupGet(x => x.Bookings).Returns(bookings.AsQueryable());
            _mockDataContext.SetupGet(x => x.Hotels).Returns(hotels.AsQueryable());

            // Act
            var result = await _roomAvailabilityService.GetRoomAvailabilityByRoomType(hotelId, availabilityPeriod, roomType, aggregated);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(7, result.Count());

            var expectedAvailability = new List<RoomAvaialabilityServiceResult>
            {
                new RoomAvaialabilityServiceResult { Day = new DateOnly(2022, 1, 1), SameCountPeriod = 1, RoomAvailabilityCount = 1 },
                new RoomAvaialabilityServiceResult { Day = new DateOnly(2022, 1, 2), SameCountPeriod = 1, RoomAvailabilityCount = 0 },
                new RoomAvaialabilityServiceResult { Day = new DateOnly(2022, 1, 3), SameCountPeriod = 1, RoomAvailabilityCount = 0 },
                new RoomAvaialabilityServiceResult { Day = new DateOnly(2022, 1, 4), SameCountPeriod = 1, RoomAvailabilityCount = 1 },
                new RoomAvaialabilityServiceResult { Day = new DateOnly(2022, 1, 5), SameCountPeriod = 1, RoomAvailabilityCount = 0 },
                new RoomAvaialabilityServiceResult { Day = new DateOnly(2022, 1, 6), SameCountPeriod = 1, RoomAvailabilityCount = 1 },
                new RoomAvaialabilityServiceResult { Day = new DateOnly(2022, 1, 7), SameCountPeriod = 1, RoomAvailabilityCount = 1 }
            };

            CollectionAssert.AreEqual(expectedAvailability, result);
        }
    }
}
