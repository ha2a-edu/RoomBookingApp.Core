using RoomBookingApp.DataServices;
using RoomBookingApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBookingApp.Persistance.Repositories
{
    public class RoomBookingService : IRoomBookingService
    {
        private readonly RoomBookingAppDbContext _dbContext;

        public RoomBookingService(RoomBookingAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Room> GetAvailableRooms(DateTime date)
        {
            //var roomIdsBookedOnDate = _dbContext.RoomBookings.Where(o=>o.Date == date).Select(o=>o.RoomId).ToList();
            //var availableRooms = _dbContext.Rooms.Where(o=> roomIdsBookedOnDate.Contains(o.Id) == false).ToList();
            //return availableRooms;

            return _dbContext.Rooms.Where(o => o.roomBookings.Any(x => x.Date == date) == false).ToList();
        }

        public void Save(RoomBooking roomBooking)
        {
            _dbContext.Add(roomBooking);
            _dbContext.SaveChanges();
        }
    }
}
