using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelTests.DALTests.EqualityComparer
{
    internal class BookEqualityComparer : IEqualityComparer<Book>
    {
        public bool Equals([AllowNull] Book? x, [AllowNull] Book? y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id
                && x.CustomerId == y.CustomerId
                && x.RoomId == y.RoomId
                && x.EndDate == y.EndDate
                && x.StartDate == y.StartDate
                && x.Price == y.Price;
        }

        public int GetHashCode([DisallowNull] Book obj)
        {
            return obj.GetHashCode();
        }
    }
}
