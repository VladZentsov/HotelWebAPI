using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelTests.DALTests.EqualityComparer
{
    public class RoomDetailsEqualityComparer:IEqualityComparer<Room>
    {
        public bool Equals([AllowNull] Room? x, [AllowNull] Room? y)
        {
            RoomEqualityComparer roomEqualityComparer = new RoomEqualityComparer();
            RoomImagesEqualityComparer roomImagesEqualityComparer = new RoomImagesEqualityComparer();

            if(!roomEqualityComparer.Equals(x, y))
                return false;

            if (x.RoomImages == null && y.RoomImages == null)
                return true;
            else if(x.RoomImages.Count!=y.RoomImages.Count)
                return false;

            bool result = true;

            for (int i = 0; i < x.RoomImages.Count; i++)
            {
                if (!roomImagesEqualityComparer.Equals(x.RoomImages.ElementAt(i), y.RoomImages.ElementAt(i)))
                {
                    result = false;
                    break;
                }
            }

            return result;

        }

        public int GetHashCode([DisallowNull] Room obj)
        {
            return obj.GetHashCode();
        }
    }
}
