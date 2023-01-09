using AutoMapper;
using BLL.Interfaces;
using BLL.Models;
using DAL.Entities;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoomService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddAsync(RoomDto model)
        {
            var room = _mapper.Map<Room>(model);

            _unitOfWork.RoomRepository.Add(room);

            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(string modelId)
        {
            await _unitOfWork.RoomRepository.DeleteByIdAsync(modelId);

            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<RoomDto>> GetAllAsync()
        {
            var rooms = await _unitOfWork.RoomRepository.GetAllAsync();

            var roomsDto = _mapper.Map<IEnumerable<Room>, IEnumerable<RoomDto>>(rooms);

            foreach (var room in roomsDto)
            {
                room.FirstDateForSettelment = await GetFirstDateForSettelment(room);
            }

            return roomsDto;
        }

        public async Task<RoomDto> GetByIdAsync(string id)
        {
            var room = await _unitOfWork.RoomRepository.GetByIdAsync(id);

            var RoomDto = _mapper.Map<RoomDto>(room);

            RoomDto.FirstDateForSettelment = await GetFirstDateForSettelment(RoomDto);

            return RoomDto;

        }

        public async Task<RoomWithImages> GetByIdWithDetailsAsync(string id)
        {
            var room = await _unitOfWork.RoomRepository.GetByIdWithDetailsAsync(id);


            var roomFullInfo = _mapper.Map<RoomWithImages>(room);

            List<string> roomImages = room.RoomImages.Select(x => x.ImgName).ToList();
            roomFullInfo.ImgNames = roomImages;

            return roomFullInfo;
        }

        public async Task<IEnumerable<RoomFullInfo>> GetBookedRoomsWithDetails()
        {
            var books = (await _unitOfWork.BookRepository.GetAllAsync()).Where(b=>b.EndDate>=DateTime.Now);

            var rooms = _mapper.Map<IEnumerable<RoomFullInfo>>((await _unitOfWork.RoomRepository.GetAllAsync()).Where(r => (books.Select(x => x.RoomId)).Contains(r.Id)));

            foreach (var room in rooms)
            {
                List<(BookDto, CustomerDto)> booksAndCustomers = new List<(BookDto, CustomerDto)>();

                var currrentRoomBooks = books.Where(b => b.RoomId == room.Id);

                foreach (var currrentRoomBookElement in currrentRoomBooks)
                {
                    var currentRoomCustomer = await _unitOfWork.CustomerRepository.GetByIdAsync(currrentRoomBookElement.CustomerId);
                    booksAndCustomers.Add((_mapper.Map<BookDto>(currrentRoomBookElement), _mapper.Map<CustomerDto>(currentRoomCustomer)));
                }
                room.BooksAndCustomersInfo = booksAndCustomers;
            }

            return rooms;
        }

        public async Task<RoomFullInfo> GetBookedRoomWithDetailsById(string id)
        {
            var books = (await _unitOfWork.BookRepository.GetAllAsync()).Where(b => b.EndDate >= DateTime.Now&&b.RoomId == id);

            var room = _mapper.Map<RoomFullInfo>(await _unitOfWork.RoomRepository.GetByIdAsync(id));

            List<(BookDto, CustomerDto)> booksAndCustomers = new List<(BookDto, CustomerDto)>();

            foreach (var book in books)
            {
                var currentRoomCustomer = await _unitOfWork.CustomerRepository.GetByIdAsync(book.CustomerId);
                booksAndCustomers.Add((_mapper.Map<BookDto>(book), _mapper.Map<CustomerDto>(currentRoomCustomer)));
            }
            room.BooksAndCustomersInfo = booksAndCustomers;

            return room;
        }

        public async Task<IEnumerable<IEnumerable<RoomsSettlement>>> GetRoomsSettlement()
        {
            var books = (await _unitOfWork.BookRepository.GetAllAsync()).Where(b => b.EndDate >= DateTime.Now);

            var rooms = _mapper.Map<IEnumerable<RoomFullInfo>>((await _unitOfWork.RoomRepository.GetAllAsync()).Where(r => (books.Select(x => x.RoomId)).Contains(r.Id)));

            List<RoomsSettlement> roomsSettlements = new List<RoomsSettlement>();

            foreach (var room in rooms)
            {
                var currrentRoomBooks = books.Where(b => b.RoomId == room.Id);

                foreach (var currrentRoomBookElement in currrentRoomBooks)
                {
                    roomsSettlements.Add(new RoomsSettlement() { Category = room.Category, IsPaymentComplete = currrentRoomBookElement.IsPaymentComplete, RoomId = room.Id, IsSettlement = true, Date = currrentRoomBookElement.StartDate, BookId = currrentRoomBookElement.Id});
                    roomsSettlements.Add(new RoomsSettlement() { Category = room.Category, IsPaymentComplete = currrentRoomBookElement.IsPaymentComplete, RoomId = room.Id, IsSettlement = false, Date = currrentRoomBookElement.EndDate, BookId = currrentRoomBookElement.Id });
                }
            }

            roomsSettlements = roomsSettlements.OrderBy(x => x.Date).ToList();

            List<List<RoomsSettlement>> twoDimentionalRoomsSettlements = new List<List<RoomsSettlement>>();
            RoomsSettlement temp = new RoomsSettlement();
            int i = -1;
            int j = 0;
            foreach (var room in roomsSettlements)
            {
                if (temp == null || temp.Date != room.Date)
                {
                    j = 0;
                    i++;
                    twoDimentionalRoomsSettlements.Add(new List<RoomsSettlement>() { room });
                    temp = room;
                }
                else
                {
                    j++;
                    twoDimentionalRoomsSettlements.ElementAt(i).Add(room);
                }

            }


            return twoDimentionalRoomsSettlements;
        }



        public async Task<RoomDto> UpdateAsync(RoomDto model)
        {
            var dest = await _unitOfWork.RoomRepository.GetByIdAsync(model.Id);

            _mapper.Map<RoomDto, Room>(model, dest);

            _unitOfWork.RoomRepository.Update(dest);

            await _unitOfWork.SaveAsync();

            return _mapper.Map< Room, RoomDto >(dest);
        }

        public async Task<IEnumerable<RoomDto>> GetAllFreeRooms(RoomFilter roomFilter)
        {
            var roomsDto = _mapper.Map<IEnumerable<RoomDto>>(await GetFilterRooms(roomFilter));

            foreach (var room in roomsDto)
            {
                room.FirstDateForSettelment = await GetFirstDateForSettelment(room);
            }

            return roomsDto;   
        }

        public async Task<IEnumerable<RoomDto>> RoomsFilterSearch(RoomFilter roomFilter)
        {
            var roomsDto = _mapper.Map<IEnumerable<RoomDto>>(await GetFilterRooms(roomFilter));

            return roomsDto;
        }

        private IEnumerable<Room> DateOverlapDetector(RoomFilter roomFilter, IEnumerable<Book> books, IEnumerable<Room> rooms)
        {
            DateTime StartDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;

            if (roomFilter.StartDate != null)
                StartDate = (DateTime)roomFilter.StartDate;

            if (roomFilter.EndDate != null)
                EndDate = (DateTime)roomFilter.EndDate;
            else
                EndDate = StartDate;

            books = books.Where(book =>
            !(roomFilter.StartDate >= book.StartDate && roomFilter.StartDate <= book.EndDate) &&
            !(roomFilter.EndDate >= book.StartDate && roomFilter.EndDate <= book.EndDate) &&
            !(book.StartDate >= roomFilter.StartDate && book.StartDate <= roomFilter.EndDate) &&
            !(book.EndDate >= roomFilter.StartDate && book.EndDate <= roomFilter.EndDate));

            IEnumerable<string> occupiedRooms = books.Select(b => b.RoomId).Distinct().ToList();

            var result = rooms.Where(room => occupiedRooms.Contains(room.Id));

            return result;
        }


        private async Task<IEnumerable<Room>> GetFilterRooms(RoomFilter roomFilter)
        {
            var rooms = await _unitOfWork.RoomRepository.GetAllAsync();
            var books = await _unitOfWork.BookRepository.GetAllAsync();

            if (roomFilter.StartPrice != null)
                rooms = rooms.Where(room => room.Price >= roomFilter.StartPrice).ToList();

            if (roomFilter.EndPrice != null)
                rooms = rooms.Where(room => room.Price <= roomFilter.EndPrice).ToList();

            if (roomFilter.VisitorsNumber != null)
                rooms = rooms.Where(r => r.VisitorsNumber == roomFilter.VisitorsNumber);

            rooms = DateOverlapDetector(roomFilter, books, rooms);

            return rooms;
        }

        private async Task<DateTime> GetFirstDateForSettelment(RoomDto room)
        {
            var roomBooks = (await _unitOfWork.BookRepository.GetAllAsync()).Where(b => b.RoomId == room.Id && b.EndDate > DateTime.Now.Date);

            DateTime FirstDateForSettelment = new DateTime();

            if (roomBooks == null || roomBooks.Count() == 0)
            {
                FirstDateForSettelment = DateTime.Now.Date;
            }
            else
            {
                roomBooks.OrderBy(r => r.StartDate);

                if (roomBooks.ElementAt(0).StartDate > DateTime.Now)
                    FirstDateForSettelment = DateTime.Now.Date;
                else
                {
                    for (int i = 0; i < roomBooks.Count() - 1; i++)
                    {
                        if (roomBooks.ElementAt(i).StartDate < roomBooks.ElementAt(i + 1).EndDate.AddDays(-1) && roomBooks.ElementAt(i).StartDate >= DateTime.Now.Date.AddDays(-1))
                        {
                            FirstDateForSettelment = roomBooks.ElementAt(i).StartDate.AddDays(1);
                            break;
                        }
                    }
                    if (room.FirstDateForSettelment == null)
                        FirstDateForSettelment = roomBooks.ElementAt(roomBooks.Count() - 1).EndDate;
                }
            }
            return FirstDateForSettelment;
        }
    }
}
