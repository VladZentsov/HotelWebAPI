using DAL.Repositories;
using HotelTests.DALTests.EqualityComparer;
using HotelTests.DALTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HotelDatabaseContext;
using DAL.UnitOfWork;
using Moq;
using DAL.Entities;
using BLL.Services;
using BLL.Models;
using FluentAssertions;
using FluentAssertions.Execution;
using DAL.Interfaces;

namespace HotelTests.BusinessTests
{
    [TestFixture]
    public class BookServiceTests
    {
        [Test]
        public async Task BookService_CreateBook_CreateBookAndCustomerFromBookModel()
        {
            //arrange
            var mockUnitOfWork = CreateMockUnitOfWorkForCreateBook("1");

            var bookService = new BookService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            BookEqualityComparer bookEqualityComparer = new BookEqualityComparer();
            CustomerEqualityComparer customerEqualityComparer = new CustomerEqualityComparer();

            var expected = UnitTestHelper.GetBookViaInfoModel("1");
            var expectedCustomer = UnitTestHelper.GetCustomerViaInfoModel("1");

            //act
            var bookToCreate = UnitTestHelper.GetBookCreateModel("1");
            bookToCreate.CustomerId = null;

            await bookService.CreateBook(bookToCreate);

            //assert
            mockUnitOfWork.Verify(x => x.BookRepository.Add(It.Is<Book>(x=> bookEqualityComparer.Equals(x,expected))), Times.Once);
            mockUnitOfWork.Verify(x => x.CustomerRepository.Add(It.Is<Customer>(x => customerEqualityComparer.Equals(x, expectedCustomer))), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }
        [Test]
        public async Task BookService_CreateBook_CreateBookFromBookModel()
        {
            //arrange
            var mockUnitOfWork = CreateMockUnitOfWorkForCreateBook("2");

            var bookService = new BookService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            BookEqualityComparer bookEqualityComparer = new BookEqualityComparer();

            var expected = UnitTestHelper.GetBookViaInfoModel("2");
            expected.Id = "1";
            expected.CustomerId = "3";
            var expectedCustomer = UnitTestHelper.GetCustomerViaInfoModel("2");

            //act
            var bookToCreate = UnitTestHelper.GetBookCreateModel("2");
            bookToCreate.CustomerId = null;

            await bookService.CreateBook(bookToCreate);

            //assert
            mockUnitOfWork.Verify(x => x.BookRepository.Add(It.Is<Book>(x => bookEqualityComparer.Equals(x, expected))), Times.Once);
            mockUnitOfWork.Verify(x => x.CustomerRepository.Add(It.IsAny<Customer>()), Times.Never);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        private Mock<IUnitOfWork> CreateMockUnitOfWorkForCreateBook(string id)
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var _mockCustomerRepository = new Mock<ICustomerRepository>();

            //var _mockCustomers = new Mock<List<Customer>>();

            //_mockCustomers.Setup(x => x).Returns(UnitTestHelper.Customers);
            //_mockCustomers.As(List<BaseEntity>)();

            //_mockCustomerRepository.As<IRepository<Customer>>();
            _mockCustomerRepository.As<IRepository<BaseEntity>>().Setup(x=>x.GetAllAsync()).ReturnsAsync(UnitTestHelper.Customers);

            _mockCustomerRepository.Setup(x => x.GetAllAsync())
                .ReturnsAsync(UnitTestHelper.Customers);

            mockUnitOfWork.Setup(x => x.CustomerRepository).Returns(_mockCustomerRepository.Object);


            mockUnitOfWork.Setup(x => x.RoomRepository.GetByIdAsync(id))
                .ReturnsAsync(UnitTestHelper.GetRoom(id));

            mockUnitOfWork.Setup(x => x.CustomerRepository.GetAllAsync())
                .ReturnsAsync(UnitTestHelper.Customers);



            mockUnitOfWork.Setup(x => x.BookRepository.Add(It.IsAny<Book>()));
            mockUnitOfWork.Setup(x => x.CustomerRepository.Add(It.IsAny<Customer>()));

            return mockUnitOfWork;
        }
        [Test]
        public async Task BookService_AddBook_AddsBook()
        {
            var expected = new Book() { Id = "3", Price = 9000, StartDate = new DateTime(2020, 8, 15), EndDate = new DateTime(2020, 9, 24), CustomerId = "3", RoomId = "4" };
            //arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork.Setup(x => x.BookRepository.Add(It.IsAny<Book>()));

            var bookService = new BookService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            BookEqualityComparer bookEqualityComparer = new BookEqualityComparer();


            //act
            await bookService.AddAsync(new BookDto() { Id = "3", Price = 9000, StartDate = new DateTime(2020, 8, 15), EndDate = new DateTime(2020, 9, 24), CustomerId = "3", RoomId = "4" });

            //assert
            mockUnitOfWork.Verify(x => x.BookRepository.Add(It.Is<Book>(x => bookEqualityComparer.Equals(x, expected))), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task BookService_GetAll_ReturnsAllBooks()
        {
            //arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork.Setup(x => x.BookRepository.GetAllAsync())
                .ReturnsAsync(UnitTestHelper.Books);

            var bookService = new BookService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());
            var expected = UnitTestHelper.BooksDto;

            //act
            var actual = await bookService.GetAllAsync();

            //assert
            actual.Should().BeEquivalentTo(expected);
            
        }

        [TestCase("1")]
        [TestCase("2")]
        public async Task BookService_GetById_ReturnsBookDto(string id)
        {
            //arrange
            var expected = UnitTestHelper.GetBookDto(id);
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(m => m.BookRepository.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UnitTestHelper.GetBook(id));

            var bookService = new BookService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            //act
            var actual = await bookService.GetByIdAsync(id);

            //assert
            actual.Should().BeEquivalentTo(expected);
        }


        [TestCase("1")]
        [TestCase("2")]
        public async Task BookService_GetFullInfoByIdAsync_BookFullInfo(string id)
        {
            //arrange
            var expected = UnitTestHelper.GetBookFullInfo(id);
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(m => m.BookRepository.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UnitTestHelper.GetBook(id));

            var bookService = new BookService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            //act
            var actual = await bookService.GetByIdAsync(id);

            //assert
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("1")]
        [TestCase("2")]
        public async Task BookService_DeleteAsync_DeletesBook(string id)
         {
            //arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(m => m.BookRepository.DeleteByIdAsync(It.IsAny<string>()));

            var bookService = new BookService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            //act
            await bookService.DeleteAsync(id);

            //assert
            mockUnitOfWork.Verify(x => x.BookRepository.DeleteByIdAsync(id), Times.Once());
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once());
        }
        [Test]
        public async Task BookService_UpdateAsync_UpdatesBook()
        {
            //arrange
            var expected = new Book() { Id = "3", Price = 9000, StartDate = new DateTime(2020, 8, 15), EndDate = new DateTime(2020, 9, 24), CustomerId = "3", RoomId = "4" };
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork.Setup(m => m.BookRepository.Update(It.IsAny<Book>()));

            var bookService = new BookService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            var book = new BookDto() { Id = "3", Price = 9000, StartDate = new DateTime(2020, 8, 15), EndDate = new DateTime(2020, 9, 24), CustomerId = "3", RoomId = "4" };
            BookEqualityComparer bookEqualityComparer = new BookEqualityComparer();

            //act
            await bookService.UpdateAsync(book);

            //assert
            mockUnitOfWork.Verify(x => x.BookRepository.Update(It.Is<Book>(x => bookEqualityComparer.Equals(x, expected))), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task BookService_GetFreeBookDates_ReturnFreeBookDates()
        {

            //arrange
            FreeBookDatesModel expected = new FreeBookDatesModel();

            List<(DateTime, DateTime)> days = new List<(DateTime, DateTime)>();

            DateTime now = DateTime.Now;

            days.Add((new DateTime(now.Year, now.Month, now.Day), new DateTime(2023, 02, 15)));
            days.Add((new DateTime(2023, 03, 25), new DateTime(2023, 05, 31)));
            days.Add((new DateTime(2023, 07, 02), new DateTime(2024, 1, 09)));

            expected.Days = days;

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(m => m.BookRepository.GetAllAsync())
                .ReturnsAsync(UnitTestHelper.Books);

            mockUnitOfWork.Setup(x => x.RoomRepository.GetByIdAsync("5"))
                .ReturnsAsync(UnitTestHelper.GetRoom("5"));


            var bookService = new BookService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            //act
            var actual = await bookService.GetFreeBookDates("5");

            //assert
            actual.Should().BeEquivalentTo(expected);
        }

    }
}
