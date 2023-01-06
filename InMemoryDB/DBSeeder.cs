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
    public class DBSeeder
    {
        public static DbContextOptions<HotelDbContext> GetUnitTestDbOptions()
        {
            var options = new DbContextOptionsBuilder<HotelDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (var context = new HotelDbContext(options))
            {
                SeedData(context);
            }

            return options;
        }

        public static IMapper CreateMapperProfile()
        {
            var myProfile = new AutomapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));

            return new Mapper(configuration);
        }

        public static DbContextOptions<HotelDbContext> GetUnitEmptyDbOptions()
        {
            var options = new DbContextOptionsBuilder<HotelDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return options;
        }


        public static void SeedData(HotelDbContext context)
        {
            context.Customers.AddRange(Customers);
            context.Rooms.AddRange(Rooms);
            context.RoomHistories.AddRange(RoomHistories);

            //context.Entry(RoomHistories).State = EntityState.Detached;

            context.Books.AddRange(Books);
            context.RoomImages.AddRange(RoomImages);


            context.SaveChanges();
        }
        public static List<Customer> Customers
        {
            get
            {
                return new List<Customer>()
                {
                    new Customer { Id = "1", Name = "Max", Surname = "Martynov", Email = "MaxM@gmil.com" },
                    new Customer { Id = "2", Name = "Artem", Surname = "Ivanov", Email = "ArtemI@gmil.com" },
                    new Customer { Id = "3", Name = "Alexey", Surname = "Petrov", Email = "AlexeyP@gmil.com" },
                    new Customer { Id = "4", Name = "Ivan", Surname = "Ostrovsky", Email = "IvanO@gmil.com" }
                };
            }
        }

        public static List<Room> Rooms
        {
            get
            {
                return new List<Room>()
                {
                    new Room { Id = "1", Category = RoomCategory.Standart, VisitorsNumber = 2, Description = "Standart room in hotel", Price = 800, ImgName="Main-Standart-1" },
                    new Room { Id = "2", Category = RoomCategory.Superior, VisitorsNumber = 3, Description = "Superior room in hotel", Price = 1200, ImgName="Main-Superior-1" },
                    new Room { Id = "3", Category = RoomCategory.Apartment, VisitorsNumber = 4, Description = "Apartment room in hotel", Price = 1700, ImgName="Main-Apartment-1" },
                    new Room { Id = "4", Category = RoomCategory.Lux, VisitorsNumber = 3, Description = "Lux room in hotel", Price = 2000, ImgName="Main-Lux-1" },
                    new Room { Id = "5", Category = RoomCategory.Duplex, VisitorsNumber = 6, Description = "Duplex room in hotel", Price = 2500, ImgName="Main-Duplex-1" }
                };
            }
        }

        public static List<RoomImages> RoomImages
        {
            get
            {
                return new List<RoomImages>
                {
                    new RoomImages{Id = "1", ImgName = "Standart-2-1", RoomId ="1"},
                    new RoomImages{Id = "2", ImgName = "Standart-2-2", RoomId ="1"},
                    new RoomImages{Id = "3", ImgName = "Standart-2-3", RoomId ="1"},
                    new RoomImages{Id = "4", ImgName = "Superior-3-1", RoomId ="2"},
                    new RoomImages{Id = "5", ImgName = "Superior-3-2", RoomId ="2"},
                    new RoomImages{Id = "6", ImgName = "Superior-3-3", RoomId ="2"},
                    new RoomImages{Id = "7", ImgName = "Apartment-2-1", RoomId ="3"},
                    new RoomImages{Id = "8", ImgName = "Lux-2-1", RoomId ="4"},
                    new RoomImages{Id = "9", ImgName = "Duplex-2-1", RoomId ="5"},
                };
            }

        }

        public static List<RoomHistory> RoomHistories
        {
            get
            {
                return new List<RoomHistory>()
                {
                    new RoomHistory { Id = "1", Price = 700, StartDate = new DateTime(2021, 8, 15), EndDate = new DateTime(2021, 8, 20), CustomerId = "1", RoomId = "1"},
                    new RoomHistory { Id = "2", Price = 800, StartDate = new DateTime(2021, 8, 25), EndDate = new DateTime(2021, 8, 28), CustomerId = "2", RoomId = "1" },

                    new RoomHistory { Id = "3", Price = 1300, StartDate = new DateTime(2021, 9, 1), EndDate = new DateTime(2021, 9, 6), CustomerId = "1", RoomId = "2" },
                    new RoomHistory { Id = "4", Price = 1200, StartDate = new DateTime(2021, 9, 7), EndDate = new DateTime(2021, 9, 12), CustomerId = "2", RoomId = "2" },

                    new RoomHistory { Id = "5", Price = 1500, StartDate = new DateTime(2021, 9, 15), EndDate = new DateTime(2021, 9, 17), CustomerId = "2", RoomId = "3" },
                    new RoomHistory { Id = "6", Price = 1600, StartDate = new DateTime(2021, 9, 18), EndDate = new DateTime(2021, 9, 25), CustomerId = "3", RoomId = "3" },

                    new RoomHistory { Id = "7", Price = 2100, StartDate = new DateTime(2021, 9, 26), EndDate = new DateTime(2021, 9, 28), CustomerId = "2", RoomId = "4" },
                    new RoomHistory { Id = "8", Price = 1900, StartDate = new DateTime(2021, 10, 2), EndDate = new DateTime(2021, 10, 7), CustomerId = "3", RoomId = "4" },

                    new RoomHistory { Id = "9", Price = 2300, StartDate = new DateTime(2021, 10, 8), EndDate = new DateTime(2021, 10, 16), CustomerId = "4", RoomId = "5" },
                    new RoomHistory { Id = "10", Price = 2400, StartDate = new DateTime(2021, 10, 16), EndDate = new DateTime(2021, 10, 25), CustomerId = "4", RoomId = "5" }

                };
            }
        }

        public static List<Book> Books
        {
            get
            {
                return new List<Book>()
                {
                    new Book { Id = "1", Price = 700, StartDate = new DateTime(2020, 8, 15), EndDate = new DateTime(2020, 8, 20), CustomerId = "1", RoomId = "1" },
                    new Book { Id = "2", Price = 800, StartDate = new DateTime(2020, 8, 25), EndDate = new DateTime(2020, 8, 28), CustomerId = "2", RoomId = "1" },

                    new Book { Id = "3", Price = 1300, StartDate = new DateTime(2020, 9, 1), EndDate = new DateTime(2020, 9, 6), CustomerId = "1", RoomId = "2" },
                    new Book { Id = "4", Price = 1200, StartDate = new DateTime(2020, 9, 7), EndDate = new DateTime(2020, 9, 12), CustomerId = "2", RoomId = "2" },

                    new Book { Id = "5", Price = 1500, StartDate = new DateTime(2020, 9, 15), EndDate = new DateTime(2020, 9, 17), CustomerId = "2", RoomId = "3" },
                    new Book { Id = "6", Price = 1600, StartDate = new DateTime(2020, 9, 18), EndDate = new DateTime(2020, 9, 25), CustomerId = "3", RoomId = "3" },

                    new Book { Id = "7", Price = 2100, StartDate = new DateTime(2020, 9, 26), EndDate = new DateTime(2023, 2, 16), CustomerId = "2", RoomId = "4" },
                    new Book { Id = "8", Price = 1900, StartDate = new DateTime(2020, 10, 2), EndDate = new DateTime(2023, 4, 7), CustomerId = "3", RoomId = "4" },

                    new Book { Id = "9", Price = 2300, StartDate = new DateTime(2023, 02, 16), EndDate = new DateTime(2023, 03, 25), CustomerId = "4", RoomId = "5" },
                    new Book { Id = "10", Price = 2400, StartDate = new DateTime(2023, 06, 1), EndDate = new DateTime(2023, 07, 2), CustomerId = "4", RoomId = "5" }

                };
            }
        }

        

    }
}
