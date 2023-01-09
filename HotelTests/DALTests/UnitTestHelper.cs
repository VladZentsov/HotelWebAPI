using AutoMapper;
using BLL;
using BLL.Models;
using DAL.Entities;
using DAL.Enums;
using DAL.HotelDatabaseContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelTests.DALTests
{
    public class UnitTestHelper
    {
        public static DbContextOptions<HotelDbContext> GetUnitTestDbOptions()
        {
            return DBSeeder.GetUnitTestDbOptions();
        }

        public static IMapper CreateMapperProfile()
        {
            return DBSeeder.CreateMapperProfile();
        }

        public static DbContextOptions<HotelDbContext> GetUnitEmptyDbOptions()
        {
            return DBSeeder.GetUnitEmptyDbOptions();
        }

        public static void SeedData(HotelDbContext context)
        {
            context.Customers.AddRange(Customers);
            context.Rooms.AddRange(Rooms);
            context.RoomHistories.AddRange(RoomHistories);

            //context.Entry(RoomHistories).State = EntityState.Detached;

            context.Books.AddRange(Books);


            context.SaveChanges();
        }
        public static Customer GetCustomer(string id)
        {
            return Customers.FirstOrDefault(x => x.Id == id);
        }

        public static Book GetBook(string id)
        {
            return Books.FirstOrDefault(x => x.Id == id);
        }

        public static BookDto GetBookDto(string id)
        {
            return BooksDto.FirstOrDefault(x => x.Id == id);
        }
        
        public static BookDto GetBookFullInfo(string id)
        {
            var mapper = DBSeeder.CreateMapperProfile();

            var book = GetBook(id);
            book.Room = GetRoom(id);
            book.Customer = GetCustomer(id);

            var result = mapper.Map<BookFullInfo>(book);

            return result;
        }
        public static Book GetBookViaInfoModel(string id)
        {
            return BooksViaBookInfoModels.FirstOrDefault(x => x.Id == id);
        }

        public static Customer GetCustomerViaInfoModel(string id)
        {
            return CustomerViaInfoModel.ElementAt(Convert.ToInt32(id)-1);
        }

        public static Room GetRoom(string id)
        {
            return Rooms.FirstOrDefault(x => x.Id == id);
        }

        public static List<Book> GetAllBooksWithDetils()
        {
            var books = Books;
            foreach (var book in books)
            {
                book.Room = GetRoom(book.RoomId);
                book.Customer = GetCustomer(book.CustomerId);
            }

            return books;
        }

        

        public static Room GetRoomWithDetils(string id)
        {
            var allRoomImages = RoomImages.Where(x => x.RoomId == id);
            var room = GetRoom(id);
            room.RoomImages = allRoomImages.ToList();

            return room;
        }

        public static RoomImages GetRoomImages(string id)
        {
            return RoomImages.FirstOrDefault(x => x.Id == id);
        }

        public static RoomDto GetRoomDto(string id)
        {
            return RoomsDto.FirstOrDefault(x => x.Id == id);
        }

        public static RoomHistory GetRoomHistory(string id)
        {
            return RoomHistories.FirstOrDefault(x => x.Id == id);
        }

        public static BookFullInfo GetBookInfoModel(string id)
        {
            return BookInfoModels.FirstOrDefault(x => x.Id == id);
        }

        public static BookCreateModel GetBookCreateModel(string id)
        {
            var mapper = DBSeeder.CreateMapperProfile();

            return mapper.Map<BookCreateModel>(BookInfoModels.FirstOrDefault(x => x.Id == id));
        }

        public static List<Customer> Customers
        {
            get
            {
                return DBSeeder.Customers;
            }
        }

        public static List<Room> Rooms
        {
            get
            {
                return DBSeeder.Rooms;
            }
        }

        public static List<RoomImages> RoomImages
        {
            get
            {
                return DBSeeder.RoomImages;
            }
        }

        public static List<RoomDto> RoomsDto
        {
            get
            {
                DateTime now = DateTime.Now;
                now = now.Date;
                return new List<RoomDto>()
                {
                    new RoomDto { Id = "1", Category = RoomCategory.Standart, VisitorsNumber = 2, Description = "Standart room in hotel", Price = 800, imgName="Main-Standart-1", FirstDateForSettelment = now },
                    new RoomDto { Id = "2", Category = RoomCategory.Superior, VisitorsNumber = 3, Description = "Superior room in hotel", Price = 1200, imgName="Main-Superior-1", FirstDateForSettelment = now },
                    new RoomDto { Id = "3", Category = RoomCategory.Apartment, VisitorsNumber = 4, Description = "Apartment room in hotel", Price = 1700, imgName = "Main-Apartment-1", FirstDateForSettelment = now },
                    new RoomDto { Id = "4", Category = RoomCategory.Lux, VisitorsNumber = 3, Description = "Lux room in hotel", Price = 2000, imgName = "Main-Lux-1", FirstDateForSettelment = new DateTime(2023, 4, 7) },
                    new RoomDto { Id = "5", Category = RoomCategory.Duplex, VisitorsNumber = 6, Description = "Duplex room in hotel", Price = 2500, imgName = "Main-Duplex-1", FirstDateForSettelment = now }
                };
            }
        }

        public static List<RoomHistory> RoomHistories
        {
            get
            {
                return DBSeeder.RoomHistories;
            }
        }

        public static List<Book> Books
        {
            get
            {
                return DBSeeder.Books;
            }
        }

        public static List<BookDto> BooksDto
        {
            get
            {
                return new List<BookDto>()
                {
                    new BookDto { Id = "1", Price = 700, StartDate = new DateTime(2020, 8, 15), EndDate = new DateTime(2020, 8, 20), CustomerId = "1", RoomId = "1", IsPaymentComplete = false },
                    new BookDto { Id = "2", Price = 800, StartDate = new DateTime(2020, 8, 25), EndDate = new DateTime(2020, 8, 28), CustomerId = "2", RoomId = "1", IsPaymentComplete = false },

                    new BookDto { Id = "3", Price = 1300, StartDate = new DateTime(2020, 9, 1), EndDate = new DateTime(2020, 9, 6), CustomerId = "1", RoomId = "2", IsPaymentComplete = false },
                    new BookDto { Id = "4", Price = 1200, StartDate = new DateTime(2020, 9, 7), EndDate = new DateTime(2020, 9, 12), CustomerId = "2", RoomId = "2", IsPaymentComplete = false },

                    new BookDto { Id = "5", Price = 1500, StartDate = new DateTime(2020, 9, 15), EndDate = new DateTime(2020, 9, 17), CustomerId = "2", RoomId = "3", IsPaymentComplete = false },
                    new BookDto { Id = "6", Price = 1600, StartDate = new DateTime(2020, 9, 18), EndDate = new DateTime(2020, 9, 25), CustomerId = "3", RoomId = "3", IsPaymentComplete = false },

                    new BookDto { Id = "7", Price = 2100, StartDate = new DateTime(2020, 9, 26), EndDate = new DateTime(2023, 2, 16), CustomerId = "2", RoomId = "4", IsPaymentComplete = false },
                    new BookDto { Id = "8", Price = 1900, StartDate = new DateTime(2020, 10, 2), EndDate = new DateTime(2023, 4, 7), CustomerId = "3", RoomId = "4", IsPaymentComplete = false },

                    new BookDto { Id = "9", Price = 2300, StartDate = new DateTime(2023, 02, 16), EndDate = new DateTime(2023, 03, 25), CustomerId = "4", RoomId = "5", IsPaymentComplete = false },
                    new BookDto { Id = "10", Price = 2400, StartDate = new DateTime(2023, 06, 1), EndDate = new DateTime(2023, 07, 2), CustomerId = "4", RoomId = "5", IsPaymentComplete = false }

                };
            }
        }

        public static List<BookFullInfo> BookInfoModels
        {
            get
            {
                return new List<BookFullInfo>()
                {
                    new BookFullInfo{Id = "1", CustomerId = "1", StartDate = new DateTime(2020, 8, 15), EndDate = new DateTime(2020, 8, 20), RoomId = "1", Email = "TestEmail1", Name = "TestName1", Surname = "TestSurname1" ,Price = 800, },
                    new BookFullInfo{Id = "2", CustomerId = "2", RoomId = "2", StartDate = new DateTime(2020, 8, 25), EndDate = new DateTime(2020, 8, 28),  Name = "Alexey", Surname = "Petrov", Email = "AlexeyP@gmil.com", Price = 1200 }
                };
            }
        }

        public static List<Book> BooksViaBookInfoModels
        {
            get
            {
                return new List<Book>()
                {
                    new Book{Id = "1", CustomerId = "5", StartDate = new DateTime(2020, 8, 15), EndDate = new DateTime(2020, 8, 20), RoomId = "1", Price = 800},
                    new Book{Id = "2", CustomerId = "2", RoomId = "2", StartDate = new DateTime(2020, 8, 25), EndDate = new DateTime(2020, 8, 28), Price = 1200 },
                    new Book{Id = "3", CustomerId = null, StartDate = new DateTime(2020, 8, 15), EndDate = new DateTime(2020, 8, 20), RoomId = "1", Price = 800},
                };
            }
        }

        public static List<Customer> CustomerViaInfoModel
        {
            get
            {
                return new List<Customer>()
                {
                    new Customer{Email = "TestEmail1", Name = "TestName1", Surname = "TestSurname1", Id = "5"},
                    new Customer { Id = "3", Name = "Alexey", Surname = "Petrov", Email = "AlexeyP@gmil.com" },
                };
            }
        }


        public static Book GetBookWithDetails(string id)
        {
            var book = GetBook(id);

            book.Customer = GetCustomer(book.CustomerId);
            book.Room = GetRoom(book.RoomId);

            return book;
        }

        public static Customer GetCustomerWithDetails(string id)
        {
            var customer = GetCustomer(id);

            var books = Books.Where(x => x.CustomerId == customer.Id).ToList();
            customer.Books = books;

            return customer;
        }

        public static RoomHistory RoomHistoryWithDetails(string id)
        {
            var roomHistory = GetRoomHistory(id);

            roomHistory.Customer = GetCustomer(roomHistory.CustomerId);
            roomHistory.Room = GetRoom(roomHistory.RoomId);

            return roomHistory;
        }
    }
}
