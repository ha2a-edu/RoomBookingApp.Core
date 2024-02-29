using Microsoft.AspNetCore.Mvc;
using Moq;
using RoomBookingApp.Api.Controllers;
using RoomBookingApp.Core.Enums;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;
using Shouldly;

namespace RoomBookingApp.Api.Tests
{
    public class RoomBookingControllerTest
    {
        private Mock<IRoomBookingRequestProcessor> _roomBookingRequestProcessor;
        private RoomBookingController _roomBookingController;
        private RoomBookingRequest _roomBookingRequest;
        private RoomBookingResponse _roomBookingResponse;

        public RoomBookingControllerTest()
        {
            _roomBookingRequestProcessor = new Mock<IRoomBookingRequestProcessor>();
            _roomBookingController = new RoomBookingController(_roomBookingRequestProcessor.Object);
            _roomBookingRequest = new RoomBookingRequest();
            _roomBookingResponse = new RoomBookingResponse();

            _roomBookingRequestProcessor.Setup(x => x.BookRoom(_roomBookingRequest)).Returns(_roomBookingResponse);
        }

        [Theory]
        [InlineData(1, true, typeof(OkObjectResult), BookingResponseFlag.Sucess)]
        [InlineData(0, false, typeof(BadRequestObjectResult), BookingResponseFlag.Failure)]
        public async Task ShouldCallBookingMethodWhenValid(int expectedMethodCalls, 
            bool isModelValid, 
            Type expectedActionResultType, 
            BookingResponseFlag flag)
        {
            // Arrange
            if (!isModelValid)
            {
                _roomBookingController.ModelState.AddModelError("Key", "ErrorMessage");
            }

            _roomBookingResponse.Flag = flag;

            // Action
            var _response = await _roomBookingController.BookRoom(_roomBookingRequest);

            // Assert
            _response.ShouldBeOfType(expectedActionResultType);
            _roomBookingRequestProcessor.Verify(x => x.BookRoom(_roomBookingRequest), Times.Exactly(expectedMethodCalls));
        }
    }
}
