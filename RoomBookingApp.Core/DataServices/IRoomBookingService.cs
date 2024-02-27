using RoomBookingApp.Core.Domain;

namespace RoomBookingApp.DataServices
{
    public interface IRoomBookingService
    {
        void Save(RoomBooking roomBooking);

        IEnumerable<Room> GetAvailableRooms(DateTime date); 
    }
}
