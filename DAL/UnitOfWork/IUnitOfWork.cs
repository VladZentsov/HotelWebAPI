using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IRoomRepository RoomRepository { get; }
        public ICustomerRepository CustomerRepository { get; }
        public IRoomHistoryRepository RoomHistoryRepository { get; }
        public IBookRepository BookRepository { get; }

        Task SaveAsync();
    }
}
