using DAL.Entities;
using DAL.HotelDatabaseContext;
using DAL.Repositories;
using HotelTests.DALTests.EqualityComparer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelTests.DALTests.RepositoryTests
{
    [TestFixture]
    public class CustomerRepositoryTests
    {

        [TestCase("1")]
        [TestCase("2")]
        public async Task CustomerRepository_GetByIdAsync_ReturnsSingleValue(string id)
        {
            //arrange
            using var context = new HotelDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var expected = UnitTestHelper.GetCustomer(id);
            var customerRepository = new CustomerRepository(context);

            //act
            var customer = await customerRepository.GetByIdAsync(id);

            //assert
            Assert.That(customer, Is.EqualTo(expected).Using(new CustomerEqualityComparer()), message: "GetByIdAsync method works incorrect");
        }

        [Test]
        public async Task CustomerRepository_GetByIdAsync_ThrowsArgumentException()
        {
            //arrange
            using var context = new HotelDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var customerRepository = new CustomerRepository(context);

            //act + assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await customerRepository.GetByIdAsync("999"));
        }

        [TestCase("1")]
        [TestCase("2")]
        public async Task CustomerRepository_GetByIdWithDetailsAsync_ReturnsAllValuesWithDetails(string id)
        {
            //arrange
            using var context = new HotelDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var expected = UnitTestHelper.GetCustomerWithDetails(id);
            var customerRepo = new CustomerRepository(context);

            //act
            var customers = await customerRepo.GetByIdWithDetailsAsync(id);

            //assert
            Assert.That(customers, Is.EqualTo(expected).Using(new CustomerDetailsEqualityComparer()), message: "GetAllAsync method works incorrect");
        }

        [Test]
        public async Task CustomerRepository_GetByIdWithDetailsAsync_ThrowArgumentException()
        {
            //arrange
            using var context = new HotelDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var customerRepo = new CustomerRepository(context);

            //act + assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await customerRepo.GetByIdWithDetailsAsync("999"));
        }

        [Test]
        public async Task CustomerRepository_GetAllAsync_ReturnsAllValues()
        {
            //arrange
            using var context = new HotelDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var expected = UnitTestHelper.Customers;
            var customerRepository = new CustomerRepository(context);

            //act
            var customers = await customerRepository.GetAllAsync();

            //assert
            Assert.That(customers, Is.EqualTo(expected).Using(new CustomerEqualityComparer()), message: "GetAllAsync method works incorrect");
        }

        [Test]
        public async Task CustomerRepository_AddAsync_AddsValueToDatabase()
        {
            //arrange
            using var context = new HotelDbContext(UnitTestHelper.GetUnitTestDbOptions());
            var customerRepository = new CustomerRepository(context);
            var customer = new Customer{ Id = "8", Name = "TestName", Surname = "TestSurname", Email = "TestSurname" };

            //act
            customerRepository.Add(customer);
            await context.SaveChangesAsync();

            //assert
            Assert.That(context.Customers.Count(), Is.EqualTo(5), message: "AddAsync method works incorrect");
        }

        [Test]
        public async Task CustomerRepository_DeleteByIdAsync_DeletesEntity()
        {
            //arrange
            using var context = new HotelDbContext(UnitTestHelper.GetUnitTestDbOptions());

            var customerRepository = new CustomerRepository(context);

            //act
            await customerRepository.DeleteByIdAsync("1");
            await context.SaveChangesAsync();

            //assert
            Assert.That(context.Customers.Count(), Is.EqualTo(3), message: "DeleteByIdSAsync works incorrect");
        }

        [Test]
        public async Task CustomerRepository_Update_UpdatesEntity()
        {
            //arrange
            using var context = new HotelDbContext(UnitTestHelper.GetUnitTestDbOptions());

            var customerRepository = new CustomerRepository(context);
            var customer = new Customer
            {
                Id = "1", 
                Email= "Update@gmail.com", 
                Name = "UbdateN",
                Surname = "UpdateN"
            };

            //act
            customerRepository.Update(customer);
            await context.SaveChangesAsync();

            //assert
            var result = context.Customers.Find("1");
            Assert.That(customer, Is.EqualTo(new Customer
            {
                Id = "1",
                Email = "Update@gmail.com",
                Name = "UbdateN",
                Surname = "UpdateN"
            }).Using(new CustomerEqualityComparer()), message: "Update method works incorrect");
        }


    }
}
