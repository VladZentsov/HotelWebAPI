using DAL.HotelDatabaseContext;
using DAL.Interfaces;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IHotelDbContext _hotelDbContext;

        private IRoomRepository _roomRepository;
        private ICustomerRepository _customerRepository;
        private IRoomHistoryRepository _roomHistoryRepository;
        private IBookRepository _bookRepository;

        public UnitOfWork(IHotelDbContext hotelDbContext)
        {
            _hotelDbContext = hotelDbContext;
        }

        public ICustomerRepository CustomerRepository
        {
            get { 
                if(_customerRepository == null)
                {
                    _customerRepository = new CustomerRepository(_hotelDbContext);
                }
                return _customerRepository;
            }
        }
        public IRoomRepository RoomRepository
        {
            get
            {
                if (_roomRepository == null)
                {
                    _roomRepository = new RoomRepository(_hotelDbContext);
                }
                return _roomRepository;
            }
        }

        public IRoomHistoryRepository RoomHistoryRepository
        {
            get
            {
                if (_roomHistoryRepository == null)
                {
                    _roomHistoryRepository = new RoomHistoryRepository(_hotelDbContext);
                }
                return _roomHistoryRepository;
            }
        }

        public IBookRepository BookRepository
        {
            get
            {
                if (_bookRepository == null)
                {
                    _bookRepository = new BookRepository(_hotelDbContext);
                }
                return _bookRepository;
            }
        }

        public Task SaveAsync()
        {
            _hotelDbContext.SaveChanges();
            return Task.CompletedTask;
        }

        //    public IRoomRepository RoomRepository
        //    {
        //        get
        //        {
        //            if (_roomRepository == null)
        //            {
        //                _roomRepository = new RoomRepository(_hotelDbContext);
        //            }
        //            return _roomRepository;
        //        }
        //    }

        //    public IRoomHistoryRepository RoomHistoryRepository => throw new NotImplementedException();

        //    public IBookRepository BookRepository => throw new NotImplementedException();
    }
}
