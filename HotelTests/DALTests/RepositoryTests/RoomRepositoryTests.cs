using DAL.Entities;
using DAL.Enums;
using DAL.HotelDatabaseContext;
using DAL.Repositories;
using HotelTests.DALTests.EqualityComparer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelTests.DALTests.RepositoryTests
{
    [TestFixture]
    public class RoomRepositoryTests
    {

        [TestCase("1")]
        [TestCase("2")]
        public async Task RoomRepository_GetByIdAsync_ReturnsSingleValue(string id)
        {
            //arrange
            using var context = new HotelDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var expected = UnitTestHelper.GetRoom(id);
            var roomRepository = new RoomRepository(context);

            //act
            var room = await roomRepository.GetByIdAsync(id);

            //assert
            Assert.That(room, Is.EqualTo(expected).Using(new RoomEqualityComparer()), message: "GetByIdAsync method works incorrect");
        }

        [TestCase("1")]
        [TestCase("2")]
         public async Task RoomRepository_GetByIdWithDetailsAsync_ReturnsRoomWithDetails(string id)
        {
            //arrange
            using var context = new HotelDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var expected = UnitTestHelper.GetRoomWithDetils(id);
            var roomRepository = new RoomRepository(context);

            //act
            var room = await roomRepository.GetByIdWithDetailsAsync(id);

            //assert
            Assert.That(room, Is.EqualTo(expected).Using(new RoomDetailsEqualityComparer()), message: "GetByIdWithDetailsAsync method works incorrect");
        }

        [Test]
        public async Task RoomRepository_GetAllAsync_ReturnsAllValues()
        {
            //arrange
            using var context = new HotelDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var expected = UnitTestHelper.Rooms;
            var roomRepository = new RoomRepository(context);

            //act
            var rooms = await roomRepository.GetAllAsync();

            //assert
            Assert.That(rooms, Is.EqualTo(expected).Using(new RoomEqualityComparer()), message: "GetAllAsync method works incorrect");
        }

        [Test]
        public async Task RoomRepository_AddAsync_AddsValueToDatabase()
        {
            //arrange
            using var context = new HotelDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var roomRepository = new RoomRepository(context);
            var room = new Room { Id = "8", Category =RoomCategory.Apartment, Description = "TestDesc", Price = 20000};

            //act
            roomRepository.Add(room);
            await context.SaveChangesAsync();

            //assert
            Assert.That(context.Rooms.Count(), Is.EqualTo(6), message: "AddAsync method works incorrect");
        }

        [Test]
        public async Task RoomRepository_DeleteByIdAsync_DeletesEntity()
        {
            //arrange
            using var context = new HotelDbContext(UnitTestHelper.GetUnitTestDbOptions());

            var roomRepository = new RoomRepository(context);

            //act
            await roomRepository.DeleteByIdAsync("1");
            await context.SaveChangesAsync();

            //assert
            Assert.That(context.Rooms.Count(), Is.EqualTo(4), message: "DeleteByIdSAsync works incorrect");
        }

        [Test]
        public async Task RoomRepository_Update_UpdatesEntity()
        {
            //arrange
            using var context = new HotelDbContext(UnitTestHelper.GetUnitTestDbOptions());

            var roomRepository = new RoomRepository(context);
            var room = new Room { Id = "1", Category = RoomCategory.Apartment, Description = "TestDesc", Price = 20000 };

            //act
            roomRepository.Update(room);
            await context.SaveChangesAsync();

            //assert
            var result = context.Rooms.Find("1");
            Assert.That(room, Is.EqualTo(new Room { Id = "1", Category = RoomCategory.Apartment, Description = "TestDesc", Price = 20000 })
                .Using(new RoomEqualityComparer()), message: "Update method works incorrect");
        }


    }
}
