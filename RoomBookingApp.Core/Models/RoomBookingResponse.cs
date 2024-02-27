using RoomBookingApp.Core.Enums;

namespace RoomBookingApp.Core.Models
{
    public class RoomBookingResponse : RoomBookingBase
    {
        public int? RoomBookingId { get; set; }
        public BookingResponseFlag Flag { get; set; }
    }
}