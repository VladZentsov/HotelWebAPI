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
    public class RoomHistoryRepository : IRoomHistoryRepository
    {
        private IHotelDbContext _dbContext;
        private readonly DbSet<RoomHistory> _roomHistories;
        public RoomHistoryRepository(IHotelDbContext dbContext)
        {
            _dbContext = dbContext;
            _roomHistories = dbContext.Set<RoomHistory>();
        }

        public void Add(RoomHistory entity)
        {
            _roomHistories.Add(entity);
        }

        public async Task DeleteByIdAsync(string id)
        {
            Delete(await GetByIdAsync(id));
        }


        public async Task<RoomHistory> GetByIdAsync(string id)
        {
            var result = new RoomHistory();

            if (_roomHistories == null)
                throw new ArgumentNullException("DbSet is null", "_books");

            result = await _roomHistories.FirstOrDefaultAsync(x => x.Id == id);

            if (result == null)
                throw new HotelNotFoundException("Given Id not found.", "id");

            return result;
        }

        public async Task<RoomHistory> GetByIdWithDetailsAsync(string id)
        {
            if (_roomHistories == null)
                throw new ArgumentNullException("DbSet is null", "_books");

            var result = await _roomHistories
                .Include(x => x.Room)
                .Include(y => y.Customer)
                .FirstOrDefaultAsync(z => z.Id == id);

            if (result == null)
            {
                throw new ArgumentException(nameof(result));
            }
            else
                return result;
        }

        public async Task<IEnumerable<RoomHistory>> GetAllAsync()
        {
            return await _roomHistories.ToListAsync();
        }

        public void Update(RoomHistory entity)
        {
            _roomHistories.Update(entity);
        }

        public void Delete(RoomHistory entity)
        {
            _roomHistories.Remove(entity);
        }
    }
}