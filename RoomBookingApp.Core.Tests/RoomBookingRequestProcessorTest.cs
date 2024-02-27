using Moq;
using RoomBookingApp.Core.Domain;
using RoomBookingApp.Core.Enums;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;
using RoomBookingApp.DataServices;
using Shouldly;

namespace RoomBookingApp.Core.Tests
{

    public class RoomBookingRequestProcessorTest
    {
        private readonly RoomBookingRequestProcessor _processor;
        private readonly RoomBookingRequest _request;
        private readonly Mock<IRoomBookingService> _roomBookingServiceMock;
        private List<Room> _availableRooms;
        public RoomBookingRequestProcessorTest()
        {
            _request = new RoomBookingRequest
            {
                FullName = "Test Name",
                Email = "tast@email.com",
                Date = DateTime.Now
            };

            _availableRooms = new List<Room>() { new Room() { Id = 1 } };
            
            _roomBookingServiceMock = new Mock<IRoomBookingService>();
            _roomBookingServiceMock.Setup(s => s.GetAvailableRooms(_request.Date)).Returns(_availableRooms);

            _processor = new RoomBookingRequestProcessor(_roomBookingServiceMock.Object);
        }

        [Fact]
        public void Should_Return_Room_Booking_Response_With_Reques_Values()
        {
            // Arrange


            // Act
            RoomBookingBase result = _processor.BookRoom(_request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_request.FullName, result.FullName);
            Assert.Equal(_request.Email, result.Email);
            Assert.Equal(_request.Date, result.Date);
        }

        [Fact]
        public void ShouldThrowExcepyionForNullRequest()
        {
            // Arrange

            // Act

            // Assert
            var exception =  Assert.Throws<ArgumentNullException>(() => _processor.BookRoom(null));
            Assert.Equal("roomBookingRequest", exception.ParamName);
        }

        [Fact]
        public void ShouldSaveRoomBookingRequest() 
        {
            RoomBooking savedRoomBooking = null;

            _roomBookingServiceMock.Setup(s => s.Save(It.IsAny<RoomBooking>()))
                .Callback<RoomBooking>(booking => 
                {
                    savedRoomBooking = booking;
                });

            _processor.BookRoom(_request);

            _roomBookingServiceMock.Verify(q => q.Save(It.IsAny<RoomBooking>()), Times.Once);

            Assert.NotNull(savedRoomBooking);
            Assert.Equal(_request.FullName, savedRoomBooking.FullName);
            Assert.Equal(_request.Email, savedRoomBooking.Email);
            Assert.Equal(_request.Date, savedRoomBooking.Date);
            Assert.Equal(_availableRooms.First().Id, savedRoomBooking.RoomId);
        }

        [Fact]
        public void ShouldNotSaveRoomBookingRequestIfNoneAvailable()
        {
            _availableRooms.Clear();
            _processor.BookRoom(_request);
            _roomBookingServiceMock.Verify(q => q.Save(It.IsAny<RoomBooking>()), Times.Never);
        }

        [Theory]
        [InlineData(BookingResponseFlag.Failure, false)]
        [InlineData(BookingResponseFlag.Sucess, true)]
        public void ShouldReturnSuccessFlagInResponse(BookingResponseFlag bookingSuccessFlag, bool isAvailable)
        {
            if (!isAvailable)
            {
                _availableRooms.Clear();
            }

            var response = _processor.BookRoom(_request);

            bookingSuccessFlag.ShouldBe(response.Flag);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(null, false)]
        public void ShouldReturnRoomBooingIdInResponse(int? roomBookingId, bool isAvailable)
        {
            if (!isAvailable)
            {
                _availableRooms.Clear();
            }
            else
            {
                _roomBookingServiceMock.Setup(s => s.Save(It.IsAny<RoomBooking>()))
                    .Callback<RoomBooking>(booking =>
                    {
                        booking.Id = roomBookingId.Value;
                    });
            }

            var response = _processor.BookRoom(_request);
            response.RoomBookingId.ShouldBe(roomBookingId);
        }
    }
}
