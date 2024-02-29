using RoomBookingApp.Core.Models;

namespace RoomBookingApp.Core.Processors
{
    public interface IRoomBookingRequestProcessor
    {
        RoomBookingResponse BookRoom(RoomBookingRequest roomBookingRequest);
    }
}