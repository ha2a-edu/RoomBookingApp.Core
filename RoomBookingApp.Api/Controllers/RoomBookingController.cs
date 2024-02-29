using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;

namespace RoomBookingApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomBookingController : ControllerBase
    {
        private readonly IRoomBookingRequestProcessor _roomBookingRequestProcessor;

        public RoomBookingController(IRoomBookingRequestProcessor roomBookingRequestProcessor)
        {
            _roomBookingRequestProcessor = roomBookingRequestProcessor;
        }

        [HttpPost]
        public async Task<IActionResult> BookRoom(RoomBookingRequest roomBookingRequest)
        {
            if (ModelState.IsValid)
            { 
                var _response = _roomBookingRequestProcessor.BookRoom(roomBookingRequest);

                if (_response.Flag == Core.Enums.BookingResponseFlag.Sucess)
                {
                    return Ok(_response);
                }

                ModelState.AddModelError(nameof(RoomBookingRequest.Date), "No rooms are available for given date");
            }

            return BadRequest(ModelState);
        }
    }
}
