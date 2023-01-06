using BLL.Models;
using Business.Interfaces;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IBookService:ICrud<BookDto>
    {
        public Task<FreeBookDatesModel> GetFreeBookDates(string roomId);
        public Task CreateBook(BookCreateModel customerBookInfoModel);
    }
}
