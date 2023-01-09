using AutoMapper;
using BLL.Interfaces;
using BLL.ModelInterfaces;
using BLL.Models;
using DAL.Entities;
using DAL.Interfaces;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task AddAsync(BookDto model)
        {
            var book = _mapper.Map<Book>(model);

            _unitOfWork.BookRepository.Add(book);

            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(string modelId)
        {
            await _unitOfWork.BookRepository.DeleteByIdAsync(modelId);

            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<BookDto>> GetAllAsync()
        {
            var allBooks = await _unitOfWork.BookRepository.GetAllAsync();

            var allBooksDto = _mapper.Map<IEnumerable<Book>, IEnumerable<BookDto>>(allBooks);

            return allBooksDto;
        }

        public async Task<BookDto> GetByIdAsync(string id)
        {
            var book = await _unitOfWork.BookRepository.GetByIdAsync(id);

            var bookDto = _mapper.Map<BookDto>(book);

            return bookDto;
        }

        public async Task<BookFullInfo> GetFullInfoByIdAsync(string id)
        {
            var book = await _unitOfWork.BookRepository.GetByIdWithDetailsAsync(id);

            var bookFullInfo = _mapper.Map<BookFullInfo>(book);

            return bookFullInfo;
        }

        public async Task<BookDto> UpdateAsync(BookDto model)
        {
            var book = _mapper.Map<Book>(model);

            _unitOfWork.BookRepository.Update(book);

           await _unitOfWork.SaveAsync();

            return _mapper.Map<Book, BookDto>(book);
        }

        public async Task CreateBook(BookCreateModel customerBookInfoModel)
        {
            Room room = await _unitOfWork.RoomRepository.GetByIdAsync(customerBookInfoModel.RoomId);

            Book book = _mapper.Map<BookCreateModel, Book>(customerBookInfoModel);

            if (customerBookInfoModel.CustomerId == null)
            {
                Customer customer = (await _unitOfWork.CustomerRepository.GetAllAsync())
                    .FirstOrDefault(c => c.Email == customerBookInfoModel.Email && c.Name == customerBookInfoModel.Name && c.Surname == customerBookInfoModel.Surname);

                if (customer == null)
                {
                    customer = _mapper.Map<Customer>(customerBookInfoModel);

                    customer.Id = await BaseDtoHelper.GetNextId((await _unitOfWork.CustomerRepository.GetAllAsync()).Select(x => x.Id));
                    _unitOfWork.CustomerRepository.Add(customer);
                }

                book.CustomerId = customer.Id;
            }
            else
                book.CustomerId = customerBookInfoModel.CustomerId;


            book.Id = await BaseDtoHelper.GetNextId((await _unitOfWork.BookRepository.GetAllAsync()).Select(x=>x.Id));

            _unitOfWork.BookRepository.Add(book);

            await _unitOfWork.SaveAsync();
        }

        public async Task<FreeBookDatesModel> GetFreeBookDates(string roomId)
        {
            var room = await _unitOfWork.RoomRepository.GetByIdAsync(roomId);

            if(room == null)
                throw new ArgumentNullException("room", "No room with given Id");

            DateTime nowDate = DateTime.Now.Date;
            var roomBooks = (await _unitOfWork.BookRepository.GetAllAsync()).Where(b => b.RoomId == roomId && b.EndDate>DateTime.Now);

            if (roomBooks == null || roomBooks.Count() == 0)
            {
                return new FreeBookDatesModel()
                {
                    Days = new List<(DateTime, DateTime)>() {
                    (nowDate, nowDate.AddYears(1))
                    }
                };
            }

            roomBooks.OrderBy(r => r.StartDate);

            DateTime previousDate = new DateTime();

            int countDateStart = 0;

            if (roomBooks.First().StartDate<DateTime.Now)
            {
                previousDate = roomBooks.First().EndDate;
                countDateStart++;
            }
            else
                previousDate = nowDate;


            List<(DateTime, DateTime)> resultOccDates = new List<(DateTime, DateTime)>();

            for (int i = countDateStart; i < roomBooks.Count(); i++)
            {
                var book = roomBooks.ElementAt(i);
                resultOccDates.Add((previousDate, book.StartDate.AddDays(-1)));

                previousDate = book.EndDate;
            }


            DateTime end = nowDate.AddYears(1);
            resultOccDates.Add((previousDate, end));

            FreeBookDatesModel freeBookDatesModel = new FreeBookDatesModel() { Days = resultOccDates };

            return freeBookDatesModel;
        }
    }
}
