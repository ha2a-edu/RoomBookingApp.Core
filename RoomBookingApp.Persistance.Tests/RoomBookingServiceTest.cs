using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RoomBookingApp.Domain;
using RoomBookingApp.Persistance.Repositories;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RoomBookingApp.Persistance.Tests
{
    public class RoomBookingServiceTest
    {
        [Fact]
        public void ShouldReturnAvailableRooms()
        {
            // Arrange
            var date = new DateTime(1978, 12, 13);
            var dbOptions = new DbContextOptionsBuilder<RoomBookingAppDbContext>().UseInMemoryDatabase("ShouldReturnAvailableRooms").Options;

            using var context = new RoomBookingAppDbContext(dbOptions);
            
            context.Add(new Room { Id = 1, Name = "Room 1" });
            context.Add(new Room { Id = 2, Name = "Room 2" });
            context.Add(new Room { Id = 3, Name = "Room 3" });

            context.Add(new RoomBooking { Id = 1, RoomId = 1, FullName = "Yazid Abughoush", Email = "abc@xyz.com", Date = date });
            context.Add(new RoomBooking { Id = 2, RoomId = 2, FullName = "Yacoub Abughoush", Email = "123@456.com", Date = date.AddDays(-1) });

            context.SaveChanges();

            var roomBookingService = new RoomBookingService(context);

            // Act
            var availableRooms = roomBookingService.GetAvailableRooms(date);

            // Assert
            availableRooms.Count().ShouldBe(2);
            availableRooms.Where(o => o.Name == "Room 1").Any().ShouldBe(false);
            availableRooms.Where(o => o.Name == "Room 2").Any().ShouldBe(true);
        }

        [Fact]
        public void ShouldSaveNewBooking()
        {
            var date = new DateTime(1978, 12, 13);

            var dbOptions = new DbContextOptionsBuilder<RoomBookingAppDbContext>()
                .UseInMemoryDatabase("ShouldSaveNewBooking")
                .Options;

            var roomBooking = new RoomBooking 
            { 
                Id = 1, 
                RoomId = 1, 
                FullName = "Yazid Abughoush", 
                Email = "abc@xyz.com", 
                Date = date
            };

            using var context = new RoomBookingAppDbContext(dbOptions);
            var roomBookingService = new RoomBookingService(context);
            roomBookingService.Save(roomBooking);

            var bookings = context.RoomBookings.ToList();
            var booking = bookings.FirstOrDefault();

            bookings.Count().ShouldBe(1);
            booking.ShouldNotBeNull();
            booking.Id.ShouldBe(1);
        }
    }
}
