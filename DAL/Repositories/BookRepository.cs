using DAL.Entities;
using DAL.HotelDatabaseContext;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class BookRepository: IBookRepository
    {
        private IHotelDbContext _dbContext;
        private readonly DbSet<Book> _books;
        public BookRepository(IHotelDbContext dbContext)
        {
            _dbContext = dbContext;
            _books = dbContext.Set<Book>();
        }

        public void Add(Book entity)
        {
            _books.Add(entity);
        }

        public async Task DeleteByIdAsync(string id)
        {
            Delete(await GetByIdAsync(id));
        }


        public async Task<Book> GetByIdAsync(string id)
        {
            var result = new Book();

            if (_books == null)
                throw new ArgumentNullException("DbSet is null", "_books");

            result = await _books.FirstOrDefaultAsync(x => x.Id == id);

            if (result == null)
                throw new ArgumentException("Given Id not found.", "id");

            return result;
        }
         
        public async Task<Book> GetByIdWithDetailsAsync(string id)
        {
            var result = new Book();

            if (_books == null)
                throw new ArgumentNullException("DbSet is null", "_books");

            result = await _books
                .Include(x => x.Room)
                .Include(y => y.Customer)
                
                .FirstOrDefaultAsync(z => z.Id == id);

            if (result == null)
                throw new ArgumentException("Given Id not found.", "id");

            return result;
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _books.ToListAsync();
        }

        public void Update(Book entity)
        {
            _books.Update(entity);
        }

        public void Delete(Book entity)
        {
            _books.Remove(entity);
        }

        public async Task<IEnumerable<Book>> GetAllWithDetailsAsync()
        {
            //var result = new Book();

            if (_books == null)
                throw new ArgumentNullException("DbSet is null", "_books");

            var result = _books
                .Include(x => x.Room)
                .Include(y => y.Customer);

            if (result == null)
                throw new ArgumentException("Given Id not found.", "id");

            return result;
        }
    }
}
