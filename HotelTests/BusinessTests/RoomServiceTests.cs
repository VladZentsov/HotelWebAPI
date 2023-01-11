using BLL.Models;
using DAL.Entities;
using DAL.UnitOfWork;
using HotelTests.DALTests.EqualityComparer;
using HotelTests.DALTests;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Services;
using DAL.Enums;
using FluentAssertions;

namespace HotelTests.BusinessTests
{
    [TestFixture]
    public class RoomServiceTests
    {
        
        [Test]
        public async Task RoomService_AddRoom_AddsRoom()
        {
            var expected = new Room() { Id = "10", Category = RoomCategory.Standart, Description = "Test room", Price = 11800 };
            //arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork.Setup(x => x.RoomRepository.Add(It.IsAny<Room>()));

            var roomService = new RoomService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            RoomEqualityComparer roomEqualityComparer = new RoomEqualityComparer();


            //act
            await roomService.AddAsync(new RoomDto() { Id = "10", Category = RoomCategory.Standart, Description = "Test room", Price = 11800 });

            //assert
            mockUnitOfWork.Verify(x => x.RoomRepository.Add(It.Is<Room>(x => roomEqualityComparer.Equals(x, expected))), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task RoomService_GetAll_ReturnsAllRooms()
        {
            //arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork.Setup(x => x.RoomRepository.GetAllAsync())
                .ReturnsAsync(UnitTestHelper.Rooms);

            mockUnitOfWork.Setup(x => x.BookRepository.GetAllAsync())
                .ReturnsAsync(UnitTestHelper.Books);

            var roomService = new RoomService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());
            var expected = UnitTestHelper.RoomsDto;

            //act
            var actual = await roomService.GetAllAsync();

            //assert
            actual.Should().BeEquivalentTo(expected);

        }

        [Test]
        public async Task RoomService_GetAllFreeRooms_ReturnsAllFreeRooms()
        {
            //arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork.Setup(x => x.RoomRepository.GetAllAsync())
                .ReturnsAsync(UnitTestHelper.Rooms);

            mockUnitOfWork.Setup(x => x.BookRepository.GetAllAsync())
                .ReturnsAsync(UnitTestHelper.Books);

            var roomService = new RoomService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            RoomFilter roomFilter = new RoomFilter() { StartDate = new DateTime(2020, 9, 5), EndDate = new DateTime(2020, 9, 18) };

            List<string> roomIds = new List<string>() { "1", "4", "5" };
            var expected = UnitTestHelper.RoomsDto.Where(r=>roomIds.Contains(r.Id));

            //act
            var actual = await roomService.GetAllFreeRooms(roomFilter);

            //assert
            actual.Should().BeEquivalentTo(expected);

        }

        [TestCase("1")]
        [TestCase("2")]
        public async Task RoomService_GetById_ReturnsRoomDto(string id)
        {
            //arrange
            var expected = UnitTestHelper.GetRoomDto(id);
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(m => m.RoomRepository.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UnitTestHelper.GetRoom(id));


            mockUnitOfWork
                .Setup(m => m.BookRepository.GetAllAsync())
                .ReturnsAsync(UnitTestHelper.Books);

            var roomService = new RoomService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            //act
            var actual = await roomService.GetByIdAsync(id);

            //assert
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("1")]
        [TestCase("2")]
        public async Task RoomService_GetByIdWithDetailsAsync_ReturnsRoomDtoWithDetails(string id)
        {
            //arrange
            var expected = UnitTestHelper.GetRoomDto(id);
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(m => m.RoomRepository.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UnitTestHelper.GetRoom(id));

            mockUnitOfWork.Setup(x => x.RoomRepository.GetAllAsync())
                .ReturnsAsync(UnitTestHelper.Rooms);

            mockUnitOfWork.Setup(x => x.BookRepository.GetAllAsync())
                .ReturnsAsync(UnitTestHelper.Books);

            var roomService = new RoomService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            //act
            var actual = await roomService.GetByIdAsync(id);

            //assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task RoomService_GetBookedRoomsWithDetails_GetAllBookedRoomsWithDetails()
        {
            //arrange
            var expected = new List<RoomFullInfo>();
            expected.Add(new RoomFullInfo
            {
                BooksAndCustomersInfo = new List<(BookDto, CustomerDto)>
                {
                    (new BookDto()
                    {
                        CustomerId = "2",
                        EndDate = new DateTime(2023, 02, 16),
                        StartDate = new DateTime(2020, 09, 26),
                        Id = "7",
                        IsPaymentComplete = false,
                        Price = 2100,
                        RoomId = "4"
                    }, new CustomerDto()
                    {
                        Id = "2",
                        Email = "ArtemI@gmil.com",
                        Name = "Artem",
                        Surname = "Ivanov"
                    }),

                    (new BookDto()
                    {
                        CustomerId = "3",
                        EndDate = new DateTime(2023, 04, 7),
                        StartDate = new DateTime(2020, 10, 2),
                        Id = "8",
                        IsPaymentComplete = false,
                        Price = 1900,
                        RoomId = "4"
                    }, new CustomerDto()
                    {
                        Id = "3",
                        Email = "AlexeyP@gmil.com",
                        Name = "Alexey",
                        Surname = "Petrov"
                    })
                },

                Category = RoomCategory.Lux,
                Description = "Lux room in hotel",
                Id = "4",
                Price = 2000,
                VisitorsNumber = 3,
                imgName = "Main-Lux-1",

            });

            expected.Add(new RoomFullInfo
            {
                BooksAndCustomersInfo = new List<(BookDto, CustomerDto)>
                {
                    (new BookDto()
                    {
                        CustomerId = "4",
                        EndDate = new DateTime(2023, 03, 25),
                        StartDate = new DateTime(2023, 02, 16),
                        Id = "9",
                        IsPaymentComplete = false,
                        Price = 2300,
                        RoomId = "5"
                    }, new CustomerDto()
                    {
                        Id = "4",
                        Email = "IvanO@gmil.com",
                        Name = "Ivan",
                        Surname = "Ostrovsky"
                    }),

                    (new BookDto()
                    {
                        CustomerId = "4",
                        EndDate = new DateTime(2023, 07, 2),
                        StartDate = new DateTime(2023, 6, 1),
                        Id = "10",
                        IsPaymentComplete = false,
                        Price = 2400,
                        RoomId = "5"
                    }, new CustomerDto()
                    {
                        Id = "4",
                        Email = "IvanO@gmil.com",
                        Name = "Ivan",
                        Surname = "Ostrovsky"
                    })
                },

                Category = RoomCategory.Duplex,
                Description = "Duplex room in hotel",
                Id = "5",
                Price = 2500,
                VisitorsNumber = 6,
                imgName = "Main-Duplex-1",

            });

            //var expected = UnitTestHelper.GetBookeRoo
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(m => m.BookRepository.GetAllWithDetailsAsync())
                .ReturnsAsync(UnitTestHelper.GetAllBooksWithDetils);

            mockUnitOfWork
                .Setup(m => m.BookRepository.GetAllAsync())
                .ReturnsAsync(UnitTestHelper.Books);

            mockUnitOfWork
                .Setup(m => m.RoomRepository.GetAllAsync())
                .ReturnsAsync(UnitTestHelper.Rooms);

            mockUnitOfWork
                .Setup(m => m.CustomerRepository.GetByIdAsync("2"))
                .ReturnsAsync(UnitTestHelper.GetCustomer("2"));

            mockUnitOfWork
                .Setup(m => m.CustomerRepository.GetByIdAsync("3"))
                .ReturnsAsync(UnitTestHelper.GetCustomer("3"));

            mockUnitOfWork
            .Setup(m => m.CustomerRepository.GetByIdAsync("4"))
            .ReturnsAsync(UnitTestHelper.GetCustomer("4"));

            var roomService = new RoomService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            //act
            var actual = await roomService.GetBookedRoomsWithDetails();

            //assert
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("1")]
        [TestCase("2")]
        public async Task RoomService_DeleteAsync_DeletesRoom(string id)
        {
            //arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(m => m.RoomRepository.DeleteByIdAsync(It.IsAny<string>()));

            var roomService = new RoomService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            //act
            await roomService.DeleteAsync(id);

            //assert
            mockUnitOfWork.Verify(x => x.RoomRepository.DeleteByIdAsync(id), Times.Once());
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once());
        }
        [Test]
        public async Task RoomService_UpdateAsync_UpdatesRoom()
        {
            //arrange
            var expected = new Room() { Id = "1", Category = RoomCategory.Standart, Description = "Test room", Price = 11800 };
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(m => m.RoomRepository.GetByIdAsync("1"))
                .ReturnsAsync(UnitTestHelper.GetRoom("1"));

            mockUnitOfWork.Setup(m => m.RoomRepository.Update(It.IsAny<Room>()));

            var roomService = new RoomService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            var room = new RoomDto  () { Id = "1", Category = RoomCategory.Standart, Description = "Test room", Price = 11800 };
            RoomEqualityComparer roomEqualityComparer = new RoomEqualityComparer();

            //act
            await roomService.UpdateAsync(room);

            //assert
            mockUnitOfWork.Verify(x => x.RoomRepository.Update(It.Is<Room>(x => roomEqualityComparer.Equals(x, expected))), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }
    }
}
