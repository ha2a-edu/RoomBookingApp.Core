using RoomBookingApp.Core.Enums;
using RoomBookingApp.Domain.BaseModels;

namespace RoomBookingApp.Core.Models
{
    public class RoomBookingResponse : RoomBookingBase
    {
        public int? RoomBookingId { get; set; }
        public BookingResponseFlag Flag { get; set; }
    }
}