using BookingData;
using BookingData.Model;
using BookingApp.Service;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace BookingAppTest;

[TestFixture]
public class RoomAvailabilityServiceTests
{
    private Mock<IDataContext> _mockDataContext;
    private RoomAvailabilityService _roomAvailabilityService;

    [SetUp]
    public void Setup()
    {
        _mockDataContext = new Mock<IDataContext>();
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
            new() { RoomType = "Standard", HotelId = "123", Arrival = new DateOnly(2022, 1, 2), Departure = new DateOnly(2022, 1, 4), RoomRate = "2" },
            new() { RoomType = "Standard", HotelId = "123", Arrival = new DateOnly(2022, 1, 5), Departure = new DateOnly(2022, 1, 6), RoomRate = "2" },
            new() { RoomType = "Deluxe",   HotelId = "123", Arrival = new DateOnly(2022, 1, 3), Departure = new DateOnly(2022, 1, 5), RoomRate = "2" },
            new() { RoomType = "Standard", HotelId = "456", Arrival = new DateOnly(2022, 1, 1), Departure = new DateOnly(2022, 1, 3), RoomRate = "2" }
        };

        var hotels = new List<Hotel>
        {
            new () { Id = "123", Name = "California", Rooms = [ new() { RoomId = "100", RoomType = "Standard" }, new Room { RoomId = "110", RoomType = "Deluxe" } ],
                RoomTypes = [
                    new () {
                            Amenities = ["All"],
                            Code = "Standard",
                            Description = "Standard",
                            Features = ["F1"]
                    }
                ]
            },
            new () { Id = "456", Name = "MoonIsNewCalifornia", Rooms = [ new() { RoomId = "100", RoomType = "ExtraOxygen" } ],
                RoomTypes = [
                    new () {
                        Amenities = ["All"],
                        Code = "Standard",
                        Description = "Standard",
                        Features = ["F1"]
                    }
                ]
            }
        };

        _mockDataContext.SetupGet(x => x.Bookings).Returns(bookings);
        _mockDataContext.SetupGet(x => x.Hotels).Returns(hotels);

        // Act
        var result = await _roomAvailabilityService.GetRoomAvailabilityByRoomType(hotelId, availabilityPeriod, roomType, aggregated);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(7));

        var expectedAvailability = new List<RoomAvaialabilityServiceResult>
        {
            new () { Day = new DateOnly(2022, 1, 1), SameCountPeriod = 1, RoomAvailabilityCount = 1 },
            new () { Day = new DateOnly(2022, 1, 2), SameCountPeriod = 1, RoomAvailabilityCount = 0 },
            new () { Day = new DateOnly(2022, 1, 3), SameCountPeriod = 1, RoomAvailabilityCount = 0 },
            new () { Day = new DateOnly(2022, 1, 4), SameCountPeriod = 1, RoomAvailabilityCount = 0 },
            new () { Day = new DateOnly(2022, 1, 5), SameCountPeriod = 1, RoomAvailabilityCount = 0 },
            new () { Day = new DateOnly(2022, 1, 6), SameCountPeriod = 1, RoomAvailabilityCount = 0 },
            new () { Day = new DateOnly(2022, 1, 7), SameCountPeriod = 1, RoomAvailabilityCount = 1 }
        };

        //CollectionAssert.AreEquivalent(expectedAvailability, result.ToList()); 
        CollectionAssert.AreEqual(expectedAvailability, result.ToList());           
    }

    [Test]
    public async Task GetRoomAvailabilityByRoomType_ShouldReturnRoomAvailabilityAggregated()
    {
        // Arrange
        string hotelId = "123";
        var availabilityPeriod = (from: new DateOnly(2022, 1, 1), to: new DateOnly(2022, 1, 7));
        string roomType = "Standard";
        bool aggregated = true;

        var bookings = new List<Booking>
        {
            new() { RoomType = "Standard", HotelId = "123", Arrival = new DateOnly(2022, 1, 2), Departure = new DateOnly(2022, 1, 4), RoomRate = "2" },
            new() { RoomType = "Standard", HotelId = "123", Arrival = new DateOnly(2022, 1, 5), Departure = new DateOnly(2022, 1, 6), RoomRate = "2" },
            new() { RoomType = "Deluxe",   HotelId = "123", Arrival = new DateOnly(2022, 1, 3), Departure = new DateOnly(2022, 1, 5), RoomRate = "2" },
            new() { RoomType = "Standard", HotelId = "456", Arrival = new DateOnly(2022, 1, 1), Departure = new DateOnly(2022, 1, 3), RoomRate = "2" }
        };

        var hotels = new List<Hotel>
        {
            new () { Id = "123", Name = "California", Rooms = [ new() { RoomId = "100", RoomType = "Standard" }, new Room { RoomId = "110", RoomType = "Deluxe" } ],
                RoomTypes = [
                    new () {
                            Amenities = ["All"],
                            Code = "Standard",
                            Description = "Standard",
                            Features = ["F1"]
                    }
                ]
            },
            new () { Id = "456", Name = "MoonIsNewCalifornia", Rooms = [ new() { RoomId = "100", RoomType = "ExtraOxygen" } ],
                RoomTypes = [
                    new () {
                        Amenities = ["All"],
                        Code = "Standard",
                        Description = "Standard",
                        Features = ["F1"]
                    }
                ]
            }
        };

        _mockDataContext.SetupGet(x => x.Bookings).Returns(bookings);
        _mockDataContext.SetupGet(x => x.Hotels).Returns(hotels);

        // Act
        var result = await _roomAvailabilityService.GetRoomAvailabilityByRoomType(hotelId, availabilityPeriod, roomType, aggregated);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(7));

        var expectedAvailability = new List<RoomAvaialabilityServiceResult>
        {
            new () { Day = new DateOnly(2022, 1, 1), SameCountPeriod = 1, RoomAvailabilityCount = 1 },
            new () { Day = new DateOnly(2022, 1, 2), SameCountPeriod = 5, RoomAvailabilityCount = 0 },
            new () { Day = new DateOnly(2022, 1, 3), SameCountPeriod = 5, RoomAvailabilityCount = 0 },
            new () { Day = new DateOnly(2022, 1, 4), SameCountPeriod = 5, RoomAvailabilityCount = 0 },
            new () { Day = new DateOnly(2022, 1, 5), SameCountPeriod = 5, RoomAvailabilityCount = 0 },
            new () { Day = new DateOnly(2022, 1, 6), SameCountPeriod = 5, RoomAvailabilityCount = 0 },
            new () { Day = new DateOnly(2022, 1, 7), SameCountPeriod = 1, RoomAvailabilityCount = 1 }
        };

        //CollectionAssert.AreEquivalent(expectedAvailability, result.ToList()); 
        CollectionAssert.AreEqual(expectedAvailability, result.ToList());
    }




}

