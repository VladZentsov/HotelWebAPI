using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelTests.DALTests.EqualityComparer
{
    internal class RoomEqualityComparer : IEqualityComparer<Room>
    {
        public bool Equals([AllowNull] Room? x, [AllowNull] Room? y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id
                && x.Price == y.Price
                && x.Category == y.Category
                && x.Description == y.Description;
        }

        public int GetHashCode([DisallowNull] Room obj)
        {
            return obj.GetHashCode();
        }
    }
}
