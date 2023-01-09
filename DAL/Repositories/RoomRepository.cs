using DAL.Entities;
using DAL.HotelDatabaseContext;
using DAL.Interfaces;
using DAL.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private IHotelDbContext _dbContext;
        private readonly DbSet<Room> _rooms;
        public RoomRepository(IHotelDbContext dbContext)
        {
            _dbContext = dbContext;
            _rooms = dbContext.Set<Room>();
        }

        public void Add(Room entity)
        {
            _rooms.Add(entity);
        }

        public async Task DeleteByIdAsync(string id)
        {
            Delete(await GetByIdAsync(id));
        }


        public async Task<Room> GetByIdAsync(string id)
        {
            var result = new Room();

            if (_rooms == null)
                throw new ArgumentNullException("DbSet is null", "_books");

            result = await _rooms.FirstOrDefaultAsync(x => x.Id == id);

            if (result == null)
                throw new HotelNotFoundException("Given Id not found.", "id");

            return _rooms.FirstOrDefault(x => x.Id == id);
        }

        public async Task<Room> GetByIdWithDetailsAsync(string id)
        {
            var result = await _rooms
                .Include(x => x.Histories)
                .Include(x=>x.RoomImages)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (_rooms == null)
                throw new ArgumentNullException("DbSet is null", "_books");

            result = await _rooms.FirstOrDefaultAsync(x => x.Id == id);

            if (result == null)
                throw new HotelNotFoundException("Given Id not found.", "id");

            return result;
        }

        public async Task<IEnumerable<Room>> GetAllAsync()
        {
            return await _rooms.ToListAsync();
        }

        public void Update(Room entity)
        {
            _rooms.Update(entity);
        }

        public void Delete(Room entity)
        {
            _rooms.Remove(entity);
        }
    }
}
