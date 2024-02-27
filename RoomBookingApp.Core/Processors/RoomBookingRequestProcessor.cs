using RoomBookingApp.Core.Domain;
using RoomBookingApp.Core.Models;
using RoomBookingApp.DataServices;

namespace RoomBookingApp.Core.Processors
{
    public class RoomBookingRequestProcessor
    {
        private readonly IRoomBookingService _roomBookingService;

        public RoomBookingRequestProcessor(IRoomBookingService roomBookingService)
        {
            _roomBookingService = roomBookingService;
        }

        public RoomBookingResponse BookRoom(RoomBookingRequest roomBookingRequest)
        {
            if (roomBookingRequest is null)
            { 
                throw new ArgumentNullException(nameof(roomBookingRequest));   
            }

            var availableRooms = _roomBookingService.GetAvailableRooms(roomBookingRequest.Date);
            var response = CreateRoomBookingObject<RoomBookingResponse>(roomBookingRequest);

            if (availableRooms.Any())
            {
                var room = availableRooms.First();

                var roomBooking = CreateRoomBookingObject<RoomBooking>(roomBookingRequest);
                roomBooking.RoomId = room.Id;

                _roomBookingService.Save(roomBooking);

                response.Flag = Enums.BookingResponseFlag.Sucess;
                response.RoomBookingId = roomBooking.Id;
            }
            else
            {
                response.Flag = Enums.BookingResponseFlag.Failure;
            }

            return response;
        }

        private T CreateRoomBookingObject<T>(RoomBookingRequest roomBookingRequest) 
            where T : RoomBookingBase, new()
        { 
            return new T 
            { 
                FullName = roomBookingRequest.FullName, 
                Email = roomBookingRequest.Email, 
                Date = roomBookingRequest.Date 
            };
        }
    }
}