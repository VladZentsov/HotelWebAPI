using DAL.Entities;
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
    public class RoomHistoryRepositoryTests
    {

        [TestCase("1")]
        [TestCase("2")]
        public async Task RoomHistoryRepository_GetByIdAsync_ReturnsSingleValue(string id)
        {
            //arrange
            using var context = new HotelDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var expected = UnitTestHelper.GetRoomHistory(id);
            var roomHistoryRepository = new RoomHistoryRepository(context);

            //act
            var roomHistory = await roomHistoryRepository.GetByIdAsync(id);

            //assert
            Assert.That(roomHistory, Is.EqualTo(expected).Using(new RoomHistoryEqualityComparer()), message: "GetByIdAsync method works incorrect");
        }

        [Test]
        public async Task RoomHistoryRepository_GetByIdAsync_ThrowsArgumentException()
        {
            //arrange
            using var context = new HotelDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var roomHistoryRepository = new RoomHistoryRepository(context);

            //act + assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await roomHistoryRepository.GetByIdAsync("999"));
        }

        [TestCase("1")]
        [TestCase("2")]
        public async Task RoomHistoryRepository_GetByIdWithDetailsAsync_ReturnsAllValuesWithDetails(string id)
        {
            //arrange
            using var context = new HotelDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var expected = UnitTestHelper.RoomHistoryWithDetails(id);
            var roomHistoryRepo = new RoomHistoryRepository(context);

            //act
            var roomHistories = await roomHistoryRepo.GetByIdWithDetailsAsync(id);

            //assert
            Assert.That(roomHistories, Is.EqualTo(expected).Using(new RoomHistoryDetailsEqualityComparer()), message: "GetAllAsync method works incorrect");
        }

        [Test]
        public async Task RoomHistoryRepository_GetByIdWithDetailsAsync_ThrowArgumentException()
        {
            //arrange
            using var context = new HotelDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var roomHistories = new RoomHistoryRepository(context);

            //act + assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await roomHistories.GetByIdWithDetailsAsync("999"));
        }

        [Test]
        public async Task RoomHistoryRepository_GetAllAsync_ReturnsAllValues()
        {
            //arrange
            using var context = new HotelDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var expected = UnitTestHelper.RoomHistories;
            var roomHistoryRepository = new RoomHistoryRepository(context);

            //act
            var roomHistorys = await roomHistoryRepository.GetAllAsync();

            //assert
            Assert.That(roomHistorys, Is.EqualTo(expected).Using(new RoomHistoryEqualityComparer()), message: "GetAllAsync method works incorrect");
        }

        [Test]
        public async Task RoomHistoryRepository_AddAsync_AddsValueToDatabase()
        {
            //arrange
            using var context = new HotelDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var roomHistoryRepository = new RoomHistoryRepository(context);
            var roomHistory = new RoomHistory { Id = "20", StartDate = new DateTime(2020, 4, 4), EndDate = new DateTime(2020, 5, 5), CustomerId = "1", RoomId = "1" };

            //act
            roomHistoryRepository.Add(roomHistory);
            await context.SaveChangesAsync();

            //assert
            Assert.That(context.RoomHistories.Count(), Is.EqualTo(11), message: "AddAsync method works incorrect");
        }

        [Test]
        public async Task RoomHistoryRepository_DeleteByIdAsync_DeletesEntity()
        {
            //arrange
            using var context = new HotelDbContext(UnitTestHelper.GetUnitTestDbOptions());

            var roomHistoryRepository = new RoomHistoryRepository(context);

            //act
            await roomHistoryRepository.DeleteByIdAsync("1");
            await context.SaveChangesAsync();

            //assert
            Assert.That(context.RoomHistories.Count(), Is.EqualTo(9), message: "DeleteByIdSAsync works incorrect");
        }

        [Test]
        public async Task RoomHistoryRepository_Update_UpdatesEntity()
        {
            //arrange
            using var context = new HotelDbContext(UnitTestHelper.GetUnitTestDbOptions());

            var roomHistoryRepository = new RoomHistoryRepository(context);
            var roomHistory = new RoomHistory { Id = "8", StartDate = new DateTime(2020, 4, 4), EndDate = new DateTime(2020, 5, 5) };

            //act
            roomHistoryRepository.Update(roomHistory);
            await context.SaveChangesAsync();

            //assert
            var result = context.RoomHistories.Find("1");
            Assert.That(roomHistory, Is.EqualTo(new RoomHistory { Id = "8", StartDate = new DateTime(2020, 4, 4), EndDate = new DateTime(2020, 5, 5) })
                .Using(new RoomHistoryEqualityComparer()), message: "Update method works incorrect");
        }
    }
}
