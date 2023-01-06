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
    public class CustomerRepository:ICustomerRepository
    {
        private IHotelDbContext _dbContext;
        private readonly DbSet<Customer> _customers;
        public CustomerRepository(IHotelDbContext dbContext)
        {
            _dbContext = dbContext;
            _customers = dbContext.Set<Customer>();
        }

        public void Add(Customer entity)
        {
            _customers.Add(entity);
        }

        public async Task DeleteByIdAsync(string id)
        {
            Delete(await GetByIdAsync(id));
        }


        public async Task<Customer> GetByIdAsync(string id)
        {
            var result = new Customer();

            if (_customers == null)
                throw new ArgumentNullException("DbSet is null", "_customers");

            result = await _customers.FirstOrDefaultAsync(x => x.Id == id);

            if (result==null)
                throw new ArgumentException("Given Id not found.", "id");

            return result;

        }

        public async Task<Customer> GetByIdWithDetailsAsync(string id)
        {
            var result = new Customer();

            if (_customers == null)
                throw new ArgumentNullException("DbSet is null", "_books");

            result = await _customers
                .Include(x => x.Books)
                .FirstOrDefaultAsync(z => z.Id == id);

            if (result == null)
                throw new ArgumentException("Given Id not found.", "id");

            return result;

        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _customers.ToListAsync();
        }

        public void Update(Customer entity)
        {
            _customers.Update(entity);
        }

        public void Delete(Customer entity)
        {
            _customers.Remove(entity);
        }
    }
}
