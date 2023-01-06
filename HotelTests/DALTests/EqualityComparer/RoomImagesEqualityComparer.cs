using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelTests.DALTests.EqualityComparer
{
    public class RoomImagesEqualityComparer : IEqualityComparer<RoomImages>
    {
        public bool Equals([AllowNull] RoomImages? x, [AllowNull] RoomImages? y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id
                && x.RoomId == y.RoomId
                && x.ImgName == y.ImgName;
        }

        public int GetHashCode([DisallowNull] RoomImages obj)
        {
            return obj.GetHashCode();
        }
    }
}
