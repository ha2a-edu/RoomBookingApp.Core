using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;
using Shouldly;

namespace RoomBookingApp.Core.Tests
{

    public class RoomBookingRequestProcessorTest
    {
        [Fact]
        public void Should_Return_Room_Booking_Response_With_Reques_Values()
        {
            // Arrange
            var _request = new RoomBookingRequest
            {
                FullName = "Test Name",
                Email = "tast@email.com",
                Date = DateTime.Now
            };

            var _processor = new RoomBookingRequestProcessor();

            // Act
            RoomBookingResponse _result = _processor.BookRoom(_request);

            // Assert
            Assert.NotNull(_result);
            Assert.Equal(_request.FullName, _result.FullName);
            Assert.Equal(_request.Email, _result.Email);
            Assert.Equal(_request.Date, _result.Date);

            _result.ShouldNotBeNull("Result should not be null");
            _result.FullName.ShouldBe(_result.FullName);
            _result.Email.ShouldBe(_result.Email);
            _result.Date.ShouldBe(_result.Date);
        }
    }
}
