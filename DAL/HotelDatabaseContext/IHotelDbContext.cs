using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.HotelDatabaseContext
{
    public interface IHotelDbContext
    {
        int SaveChanges();
        DbSet<T> Set<T>() where T : class;
    }
}
