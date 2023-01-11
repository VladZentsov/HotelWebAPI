//using Microsoft.AspNetCore.Mvc.Testing;
using DAL.HotelDatabaseContext;
using HotelTests.DALTests;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProject;

namespace HotelTests.IntegrationTests
{
    internal class HotelWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                RemoveLibraryDbContextRegistration(services);

                var serviceProvider = GetInMemoryServiceProvider();

                services.AddDbContextPool<HotelDbContext>(options =>
                {
                    options.UseInMemoryDatabase(Guid.Empty.ToString());
                    options.UseInternalServiceProvider(serviceProvider);
                });

                //using (var scope = services.BuildServiceProvider().CreateScope())
                //{
                //    var context = scope.ServiceProvider.GetRequiredService<HotelDbContext>();

                //    UnitTestHelper.SeedData(context);
                //}
            });
        }

        private static ServiceProvider GetInMemoryServiceProvider()
        {
            return new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();
        }

        private static void RemoveLibraryDbContextRegistration(IServiceCollection services)
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<HotelDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
        }

    }
}
