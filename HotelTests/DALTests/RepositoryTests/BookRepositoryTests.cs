using DAL.Entities;
using DAL.HotelDatabaseContext;
using DAL.Repositories;
using DAL.UnitOfWork;
using HotelTests.DALTests.EqualityComparer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HotelTests.DALTests.RepositoryTests
{
    public class BookRepositoryTests
    {
        [TestCase("1")]
        [TestCase("2")]
        public async Task BookRepository_GetByIdAsync_ReturnsSingleValue(string id)
        {
            //arrange
            using var context = new HotelDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var expected = UnitTestHelper.GetBook(id);
            var bookRepository = new BookRepository(context);

            //act
            var book = await bookRepository.GetByIdAsync(id);

            //assert
            Assert.That(book, Is.EqualTo(expected).Using(new BookEqualityComparer()), message: "GetByIdAsync method works incorrect");
        }

        [Test]
        public async Task BookRepository_GetByIdAsync_ThrowsArgumentException()
        {
            //arrange
            using var context = new HotelDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var bookRepository = new BookRepository(context);

            //act + assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await bookRepository.GetByIdAsync("999"));
        }

        [TestCase("1")]
        [TestCase("2")]
        public async Task BookRepository_GetByIdWithDetailsAsync_ReturnsAllValuesWithDetails(string id)
        {
            //arrange
            using var context = new HotelDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var expected = UnitTestHelper.GetBookWithDetails(id);
            var bookRepository = new BookRepository(context);

            //act
            var books = await bookRepository.GetByIdWithDetailsAsync(id);

            //assert
            Assert.That(books, Is.EqualTo(expected).Using(new BookDetailsEqualityComparer()), message: "GetAllAsync method works incorrect");
        }

        [Test]
        public async Task BookRepository_GetByIdWithDetailsAsync_ThrowArgumentException()
        {
            //arrange
            using var context = new HotelDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var bookRepository = new BookRepository(context);

            //act + assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await bookRepository.GetByIdWithDetailsAsync("999"));
        }

        [Test]
        public async Task BookRepository_GetAllAsync_ReturnsAllValues()
        {
            //arrange
            using var context = new HotelDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var expected = UnitTestHelper.Books;
            var bookRepository = new BookRepository(context);

            //act
            var books = await bookRepository.GetAllAsync();

            //assert
            Assert.That(books, Is.EqualTo(expected).Using(new BookEqualityComparer()), message: "GetAllAsync method works incorrect");
        }

        [Test]
        public async Task BookRepository_AddAsync_AddsValueToDatabase()
        {
            //arrange
            using var context = new HotelDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var bookRepository = new BookRepository(context);
            var book = new Book { Id = "20", CustomerId = "1", RoomId = "1", StartDate = new DateTime(2019, 2, 2), EndDate = new DateTime(2019, 3, 3), Price = 30000 };

            //act
            bookRepository.Add(book);
            await context.SaveChangesAsync();

            //assert
            Assert.That(context.Books.Count(), Is.EqualTo(11), message: "AddAsync method works incorrect");
        }

        [Test]
        public async Task BookRepository_DeleteByIdAsync_DeletesEntity()
        {
            //arrange
            using var context = new HotelDbContext(UnitTestHelper.GetUnitTestDbOptions());

            var bookRepository = new BookRepository(context);

            //act
            await bookRepository.DeleteByIdAsync("1");
            await context.SaveChangesAsync();

            //assert
            Assert.That(context.Books.Count(), Is.EqualTo(9), message: "DeleteByIdSAsync works incorrect");
        }

        [Test]
        public async Task BookRepository_Update_UpdatesEntity()
        {
            //arrange
            using var context = new HotelDbContext(UnitTestHelper.GetUnitTestDbOptions());

            var bookRepository = new BookRepository(context);
            var book = new Book { Id = "5", CustomerId = "1", RoomId = "1", StartDate = new DateTime(2019, 2, 2), EndDate = new DateTime(2019, 3, 3), Price = 30000 };
            //act
            bookRepository.Update(book);
            await context.SaveChangesAsync();

            //assert
            var result = context.Books.Find("1");
            Assert.That(book, Is.EqualTo(new Book { Id = "5", CustomerId = "1", RoomId = "1", StartDate = new DateTime(2019, 2, 2), EndDate = new DateTime(2019, 3, 3), Price = 30000 })
                .Using(new BookEqualityComparer()), message: "Update method works incorrect");
        }
    }
}
