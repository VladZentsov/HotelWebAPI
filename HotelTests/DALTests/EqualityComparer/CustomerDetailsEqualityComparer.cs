using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelTests.DALTests.EqualityComparer
{
    internal class CustomerDetailsEqualityComparer : IEqualityComparer<Customer>
    {
        public bool Equals([AllowNull]  Customer? x, [AllowNull]  Customer? y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;
            BookEqualityComparer bookEqualityComparer = new BookEqualityComparer();

            var xBooks = x.Books.ToList();
            var yBooks = y.Books.ToList();

            if (xBooks.Count != yBooks.Count)
                return false;

            for (int i = 0; i < xBooks.Count; i++)
            {
                if (bookEqualityComparer.Equals(xBooks.ElementAt(i), yBooks.ElementAt(i)) == false)
                    return false;
            }

            return x.Id == y.Id
                && x.Email == y.Email;
        }

        public int GetHashCode([DisallowNull] Customer obj)
        {
            return obj.GetHashCode();
        }
    }
}
