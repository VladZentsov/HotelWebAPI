using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelTests.DALTests.EqualityComparer
{
    internal class RoomHistoryEqualityComparer : IEqualityComparer<RoomHistory>
    {
        public bool Equals([AllowNull] RoomHistory? x, [AllowNull] RoomHistory? y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id
                && x.Price == y.Price
                && x.StartDate == y.StartDate
                && x.EndDate == y.EndDate
                && x.CustomerId == y.CustomerId
                && x.RoomId == y.RoomId;
        }

        public int GetHashCode([DisallowNull] RoomHistory obj)
        {
            return obj.GetHashCode();
        }
    }
}
